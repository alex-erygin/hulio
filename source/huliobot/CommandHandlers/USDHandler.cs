using System;
using System.Net;
using System.Threading.Tasks;
using huliobot.Contracts;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    /// <summary>
    /// Показывает курс доллара.
    /// </summary>
    public class USDHandler : ICommandHandler
    {
        public async void Handle(Api botApi, Update update)
        {
            try
            {
                await botApi.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                var moneyString = await GetUsdExchange();
                var money = JsonConvert.DeserializeObject<Money>(moneyString);
                await botApi.SendTextMessage(update.Message.Chat.Id, $"{money.quotes.USDRUB.ToString("####.00")}");
            }
            catch (Exception ex)
            {
                await botApi.SendTextMessage(update.Message.Chat.Id, "Ошибочка. " + ex.Message);
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