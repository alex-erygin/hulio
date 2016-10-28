using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace huliobot
{
    public static class MySettings
    {
        /// <summary>
        /// Токен от Telegram.
        /// </summary>
        public static string TelegramToken => SettingsStore.Settings["hulio-token"];

        /// <summary>
        /// Токен от сервиса курса валют.
        /// </summary>
        public static string CurrencyApiKey => SettingsStore.Settings["currencylayerApiKey"];

        /// <summary>
        /// Токен от сервиса погоды.
        /// </summary>
        public static string WeatherApiKey => SettingsStore.Settings["openWeatherMapId"];
    }

    /// <summary>
    ///     Sends some usefult metrics by request.
    /// </summary>
    public class HulioBot : IBot
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Api bot;
        private static int offset;
        private readonly Dictionary<string, ICommandHandler> commandHandlers = new Dictionary<string, ICommandHandler>();
        private readonly MyLogger pipboy = new MyLogger("#pipboy");
        private readonly TextMessageProcessor textMessageProcessor;

        public HulioBot()
        {
            bot = new Api(MySettings.TelegramToken);
            textMessageProcessor = new TextMessageProcessor(bot);

            //http://stackoverflow.com/questions/4926676/mono-webrequest-fails-with-https
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        
        public async Task Start()
        {
            var me = await bot.GetMe();
            Logger.Debug($"{me.Username} started");

            while (true)
            {
                try
                {
                    var updates = await bot.GetUpdates(offset);

                    foreach (var update in updates)
                    {
                        switch (update.Message.Type)
                        {
                            case MessageType.TextMessage:
                            {
                                await textMessageProcessor.ProcessTextMessage(update.Message);
                            }
                                break;

                            case MessageType.PhotoMessage:
                            {
                                await ProcessPhotoMessage(update.Message);
                            }
                                break;
                        }


                        offset = update.Id + 1;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "something goes wrong");
                }
                await Task.Delay(1000);
            }
        }

        private async Task ProcessPhotoMessage(Message message)
        {
            var biggestPhoto = message.Photo.OrderByDescending(x => x.Width).FirstOrDefault();
            if (biggestPhoto == null)
                return;

            var filePath = $@"content\{DateTime.Now:dd.MM.yyyy_HH.mm.ss}.png";

            using (var stream = File.Create($@"C:\apps\fserver\{filePath}"))
            {
                await bot.GetFile(biggestPhoto.FileId, stream);
                stream.Flush();
            }

            string url = $@"http://193.124.186.83:3579/{filePath.Replace(@"\", "/")}";
            pipboy.Debug(url);
            await bot.SendTextMessage(SettingsStore.Settings["chatId"], url);
        }
    }


    public class TextMessageProcessor
    {
        private readonly Api bot;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, ICommandHandler> commandHandlers = new Dictionary<string, ICommandHandler>();
        private readonly MyLogger pipboy = new MyLogger("#pipboy");
        private readonly MyLogger todo = new MyLogger("#todo");

        public TextMessageProcessor(Api bot)
        {
            this.bot = bot;
            commandHandlers[Commands.Statistics] = new StatisticsCommandHandler();
            commandHandlers[Commands.Weather] = new WeatherHandler();
            commandHandlers[Commands.USD] = new USDHandler();
        }

        public async Task ProcessTextMessage(Message message)
        {
            var key = message.Text.ToUpper();
            if (commandHandlers.ContainsKey(key))
            {
                try
                {
                    commandHandlers[key].Handle(bot, message);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error {key}");
                }
            }
            else
            {
                if (message.Text.ToUpper().Contains("ПРИВЕТ"))
                {
                    await bot.SendTextMessage(SettingsStore.Settings["chatId"], "Привет");
                }
                if (message.Text.ToUpper().Contains("TODO"))
                {
                    todo.Debug(message.Text);
                    await bot.SendTextMessage(SettingsStore.Settings["chatId"], "Схавал");
                }
                else
                {
                    pipboy.Debug(message.Text);
                    await bot.SendTextMessage(SettingsStore.Settings["chatId"], "Unknown command");
                }
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