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
            var url = $"http://api.openweathermap.org/data/2.5/weather?id=524901&appid={SettingsStore.Settings["openWeatherMapId"]}&units=metric";
            var client = new WebClient { Encoding = Encoding.UTF8 };

            var weatherJson = client.DownloadString(url);
            Rootobject weather = JsonConvert.DeserializeObject<Rootobject>(weatherJson);
            await botApi.SendTextMessage(SettingsStore.Settings["chatId"], BuildMessage(weather).ToString());
        }


        private static StringBuilder BuildMessage(Rootobject todayWeather)
        {
            var result = new StringBuilder();
            result.AppendLine($"������, ������ �� �������: {todayWeather.weather[0].description}");
            result.AppendLine($"�����������: {todayWeather.main.temp}");
            result.AppendLine($"���������: {todayWeather.main.humidity}");
            result.AppendLine($"�������: {todayWeather.wind.speed}");
            return result;
        }
    }
}
