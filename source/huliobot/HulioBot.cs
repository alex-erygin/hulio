using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    /// <summary>
    ///     Sends some usefult metrics by request.
    /// </summary>
    public class HulioBot : IBot
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, ICommandHandler> commandHandlers = new Dictionary<string, ICommandHandler>();

        private static int offset;

        public HulioBot()
        {
            commandHandlers[Commands.Statistics] = new StatisticsCommandHandler();
            commandHandlers[Commands.Weather] = new WeatherHandler();
            commandHandlers[Commands.USD] = new USDHandler();

			//http://stackoverflow.com/questions/4926676/mono-webrequest-fails-with-https
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors)
			{
				return true;
			};
        }

        public async Task Start()
        {
            try
            {
                var token = SettingsStore.Settings["hulio-token"];
                var bot = new Api(token);
                var me = await bot.GetMe();
                Logger.Debug($"{me.Username} на связи");

                while (true)
                {
                    var updates = await bot.GetUpdates(offset);

                    foreach (var update in updates)
                    {
                        switch (update.Message.Type)
                        {
                            case MessageType.TextMessage:
                            {
                                var key = update.Message.Text.ToUpper();
                                if (commandHandlers.ContainsKey(key))
                                {
                                    try
                                    {
                                        commandHandlers[key].Handle(bot, update);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, $"Случилась ошибка при обрпботке запроса {key}");
                                    }
                                }
                                else
                                {
                                    await bot.SendTextMessage(SettingsStore.Settings["chatId"], "Я вас не понимаю");
                                }
                            }
                                break;
                        }

                        offset = update.Id + 1;
                    }
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private class Commands
        {
            public static string Statistics => "/STATS";

            public static string Weather => "/POGODA";

            public static string USD => "/USD";
        }
    }
}
