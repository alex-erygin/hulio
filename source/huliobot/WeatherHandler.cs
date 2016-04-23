using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using huliobot.Contracts;
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
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(200, i => TimeSpan.FromSeconds(1));

            await policy.Execute(() =>
            {
                var todayWeather = GetTodayWeather();
                var result = BuildMessage(todayWeather);
                return botApi.SendTextMessage(SettingsStore.Settings["chatId"], result.ToString());
            });
        }


        private static StringBuilder BuildMessage(fact todayWeather)
        {
            var result = new StringBuilder();
            result.AppendLine($"Привет, погода на сегодня: {todayWeather.weather_type}");
            result.AppendLine($"Температура: {todayWeather.temperature.Value}");
            result.AppendLine($"Влажность: {todayWeather.humidity}");
            result.AppendLine($"Ветрище: {todayWeather.wind_speed}");
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
