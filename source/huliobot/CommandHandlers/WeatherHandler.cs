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
            result.AppendLine($"Привет, погода на сегодня: {todayWeather.weather[0].description}");
            result.AppendLine($"Температура: {todayWeather.main.temp}");
            result.AppendLine($"Влажность: {todayWeather.main.humidity}");
            result.AppendLine($"Ветрище: {todayWeather.wind.speed}");
            return result;
        }

        private static fact GetTodayWeather()
        {
            var client = new WebClient {Encoding = Encoding.UTF8};
            var weatherXml = client.DownloadString("http://export.yandex.ru/weather-ng/forecasts/27612.xml");
            var xml = XDocument.Parse(weatherXml)
                .Root.Elements()
                .SingleOrDefault(x => x.Name.LocalName == "fact");

            var serializer = new XmlSerializer(typeof(fact));
            var todayWeather =
                (fact) serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString())));
            return todayWeather;
        }
    }
}
