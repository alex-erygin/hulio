using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using huliobot.Contracts;
using Newtonsoft.Json;
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
            result.AppendLine($"Weather description: {todayWeather.weather[0].description}");
            result.AppendLine($"Temperature: {todayWeather.main.temp}");
            result.AppendLine($"Humidity: {todayWeather.main.humidity}");
            result.AppendLine($"Wind speed: {todayWeather.wind.speed}");
            return result;
        }
    }
}
