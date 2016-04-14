using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using SafeConfig;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    public class HulioBot
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, Action<Api, Update>> commandHandlers =
            new Dictionary<string, Action<Api, Update>>();

        private ConfigManager configManager;
        private int offset;

        public HulioBot()
        {
            commandHandlers[Commands.Statistics] = OnFm;
            configManager = new ConfigManager().WithCurrentUserScope().Load();
            offset = configManager.Get<int>(nameof(offset));
        }

        public void Start()
        {
            Run().Wait();
        }

        public void Stop()
        {
        }

        private async void OnFm(Api api, Update update)
        {
            //get rep on stackoverflow
            StringBuilder response = new StringBuilder();

            await StackOverflowRep(response);
            await Hacker(response);
            await Nuget(response);

            await api.SendTextMessage(update.Message.Chat.Id, response.ToString());
        }

        private static async Task Nuget(StringBuilder response)
        {
            HttpClient client = new HttpClient();
            string page = await client.GetStringAsync(@"https://www.nuget.org/profiles/AlexErygin");
            Regex regex = new Regex(@"<p\s+class=""stat-number"">(?<rep>\d{1,6})</p>");
            MatchCollection matches = regex.Matches(page);
            Match match = matches.Cast<Match>().LastOrDefault(x => x.Success);
            if (match != null)
            {
                response.AppendLine($"Nuget: {match.Groups["rep"].Value}");
            }
        }
        
        private static async Task StackOverflowRep(StringBuilder response)
        {
            HttpClient client = new HttpClient();
            string page = await client.GetStringAsync(@"http://stackoverflow.com/users/1549113/alex-erygin");
            Regex regex = new Regex(@"title=""reputation"">\s+(?<rep>\d{1,6})\s+<span");
            MatchCollection matches = regex.Matches(page);
            Match match = matches.Cast<Match>().Where(x => x.Success).FirstOrDefault();
            if (match != null)
            {
                response.AppendLine($"stack: {match.Groups["rep"].Value}");
            }
        }

        private static async Task Hacker(StringBuilder response)
        {
            HttpClient client = new HttpClient();
            string page = await client.GetStringAsync(@"https://xakep.ru/2016/03/11/pki/");
            Regex regex = new Regex(@"<span class=""numcount"">(?<views>\d{1,6})</span>");
            MatchCollection matches = regex.Matches(page);
            Match match = matches.Cast<Match>().Where(x => x.Success).FirstOrDefault();
            if (match != null)
            {
                response.AppendLine($"X-PKI-views: {match.Groups["views"].Value}");
            }
        }

        public async Task Run()
        {
            try
            {
                string token = SettingsStore.Tokens["hulio-token"];
                Api bot = new Api(token);
                User me = await bot.GetMe();
                Logger.Debug($"{me.Username} на связи");

                int offset = configManager.Load().Get<int>("offset");
                while (true)
                {
                    Update[] updates = await bot.GetUpdates(offset);

                    foreach (Update update in updates)
                    {
                        switch (update.Message.Type)
                        {
                            case MessageType.TextMessage:
                            {
                                if (commandHandlers.ContainsKey(update.Message.Text))
                                {
                                    commandHandlers[update.Message.Text](bot, update);
                                }
                            }
                                break;
                        }

                        offset = update.Id + 1;
                    }
                    configManager.Set(nameof(offset), offset).Save();
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
            /// <summary>
            ///     Выводит статистику.
            /// </summary>
            public static string Statistics => "/fm";
        }
    }
}