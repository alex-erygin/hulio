using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using SafeConfig;
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

        private readonly ConfigManager configManager;
        private readonly int offset;

        public HulioBot()
        {
            commandHandlers[Commands.Statistics] = new StatisticsCommandHandler();
            commandHandlers[Commands.Weather] = new WeatherHandler();

            configManager = new ConfigManager().WithCurrentUserScope()
                .Load();
            offset = configManager.Get<int>(nameof(offset));
        }

        public async Task Start()
        {
            try
            {
                var token = SettingsStore.Settings["hulio-token"];
                var bot = new Api(token);
                var me = await bot.GetMe();
                Logger.Debug($"{me.Username} на связи");

                var offset = configManager.Load()
                    .Get<int>("offset");
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
                                    commandHandlers[key].Handle(bot, update);
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
                    configManager.Set(nameof(offset), offset)
                        .Save();
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
            public static string Statistics => "/FM";

            public static string Weather => "/POGODA";
        }
    }
}
