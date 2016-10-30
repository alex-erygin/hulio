using System;
using System.Net;
using System.Threading.Tasks;
using huliobot.Contracts;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace huliobot
{
    /// <summary>
    /// Shows UDS/RUB.
    /// </summary>
    public class USDHandler : ICommandHandler
    {
        private static readonly MyLogger Logger = new MyLogger("#usd");

        public async void Handle(TelegramBotClient botApi, Message message)
        {
            try
            {
                Logger.Debug("USD command handing begins");
                await botApi.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                var moneyString = await GetUsdExchange();
                var money = JsonConvert.DeserializeObject<Money>(moneyString);

                Logger.Debug($"Result is {money.quotes.USDRUB:####.00}");
                await botApi.SendTextMessageAsync(message.Chat.Id, $"{money.quotes.USDRUB:####.00}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "USD goes wrong");
                await botApi.SendTextMessageAsync(message.Chat.Id, "Error. " + ex.Message);
            }
        }

        private async Task<string> GetUsdExchange()
        {
            var client = new WebClient();
            var content = await client.DownloadStringTaskAsync($"http://apilayer.net/api/live?access_key={MySettings.CurrencyApiKey}&currencies=USD,RUB");
            return content;
        }
    }
}