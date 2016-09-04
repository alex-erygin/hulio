using System;
using System.Net;
using System.Threading.Tasks;
using huliobot.Contracts;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    /// <summary>
    /// Shows UDS/RUB.
    /// </summary>
    public class USDHandler : ICommandHandler
    {
        private static readonly MyLogger Logger = new MyLogger("#usd");

        public async void Handle(Api botApi, Update update)
        {
            try
            {
                Logger.Debug("USD command handing begins");
                await botApi.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                var moneyString = await GetUsdExchange();
                var money = JsonConvert.DeserializeObject<Money>(moneyString);

                Logger.Debug($"Result is {money.quotes.USDRUB:####.00}");
                await botApi.SendTextMessage(update.Message.Chat.Id, $"{money.quotes.USDRUB:####.00}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "USD goes wrong");
                await botApi.SendTextMessage(update.Message.Chat.Id, "Error. " + ex.Message);
            }
        }

        private async Task<string> GetUsdExchange()
        {
            var client = new WebClient();
            var content = await client.DownloadStringTaskAsync($"http://apilayer.net/api/live?access_key={SettingsStore.Settings["currencylayerApiKey"]}&currencies=USD,RUB");
            return content;
        }
    }
}