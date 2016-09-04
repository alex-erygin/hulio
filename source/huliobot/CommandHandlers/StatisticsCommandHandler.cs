using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    public class StatisticsCommandHandler : ICommandHandler
    {
        public async void Handle(Api botApi, Update update)
        {
            try
            {
                MyLogger.Debug("Statistics command processing started");
                StringBuilder response = new StringBuilder();
                await StackOverflowRep(response);
                await Hacker(response);
                await Nuget(response);
                MyLogger.Debug($"Statistics command result: {response}");
                await botApi.SendTextMessage(update.Message.Chat.Id, response.ToString());
            }
            catch (Exception ex)
            {
                MyLogger.Error(ex, "Something goes wrong during statistics handling");
                await botApi.SendTextMessage(update.Message.Chat.Id, ex.Message);
            }
        }

        private static async Task Nuget(StringBuilder response)
        {
            HttpClient client = new HttpClient();
            string page = await client.GetStringAsync(@"https://www.nuget.org/profiles/AlexErygin");
            page = page.Replace(",", string.Empty);
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
            Regex regex = new Regex(@"title=""reputation"">\s+(?<rep>.{1,6})\s+<span");
            MatchCollection matches = regex.Matches(page);
            Match match = matches.Cast<Match>().Where(x => x.Success).FirstOrDefault();
            if (match != null)
            {
                response.AppendLine($"stack: {match.Groups[1].Value}");
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
    }
}