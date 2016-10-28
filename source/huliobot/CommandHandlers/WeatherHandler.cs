using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using huliobot.Contracts;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    /// <summary>
    ///     Sends today`s weather
    /// </summary>
    public class WeatherHandler : ICommandHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async void Handle(Api botApi, Message message)
        {
            Logger.Debug("Weather command hadling begins");
            var chatId = SettingsStore.Settings["chatId"];
            try
            {
                await DoSendWeather(botApi, chatId);
                Logger.Debug("Weather is ok");
            }
            catch (Exception ex)
            {
                Logger.Error("Something wrong");
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
                $"http://api.openweathermap.org/data/2.5/weather?id=524901&appid={MySettings.WeatherApiKey}&units=metric";
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
