using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using huliobot.Contracts;
using Newtonsoft.Json;
using NLog;
using Polly;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    /// <summary>
    ///     Sends today`s weather
    /// </summary>
    public class WeatherHandler : ICommandHandler
    {
        public async void Handle(Api botApi, Update update)
        {
            var chatId = SettingsStore.Settings["chatId"];
            try
            {
                await DoSendWeather(botApi, chatId);
            }
            catch (Exception ex)
            {
                await SendError(botApi, chatId, ex);
            }
        }

        private static async Task SendError(Api botApi, string chatId, Exception ex)
        {
            await botApi.SendTextMessage(chatId, ex.Message);
        }

        private static async Task DoSendWeather(Api botApi, string chatId)
        {
            var url =
                $"http://api.openweathermap.org/data/2.5/weather?id=524901&appid={SettingsStore.Settings["openWeatherMapId"]}&units=metric";
            var client = new WebClient {Encoding = Encoding.UTF8};

            var weatherJson = client.DownloadString(url);
            Rootobject weather = JsonConvert.DeserializeObject<Rootobject>(weatherJson);
            await botApi.SendChatAction(chatId, ChatAction.Typing);
            await botApi.SendTextMessage(chatId, BuildMessage(weather)
                .ToString());
        }


        private static StringBuilder BuildMessage(Rootobject todayWeather)
        {
            var result = new StringBuilder();
            result.AppendLine($"Привет, погода на сегодня: {todayWeather.weather[0].description}");
            result.AppendLine($"Температура: {todayWeather.main.temp}");
            result.AppendLine($"Влажность: {todayWeather.main.humidity}");
            result.AppendLine($"Ветрище: {todayWeather.wind.speed}");
            return result;
        }
    }
}
