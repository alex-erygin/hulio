using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using huliobot.Contracts;
using Polly;
using Telegram.Bot;

namespace huliobot
{
    /// <summary>
    ///     Sends today`s weather
    /// </summary>
    public class WeatherBot : IBot
    {
        private static TimeSpan actionTime = new TimeSpan(7,30,0);

        private static Api Bot
        {
            get
            {
                var token = SettingsStore.Settings["hulio-token"];
                var bot = new Api(token);
                return bot;
            }
        }

        public async Task Start()
        {
            while (true)
            {
                var now = DateTime.Now;
                if (now.Hour == actionTime.Hours && now.Minute == actionTime.Minutes)
                {


                    var policy = Policy
                        .Handle<Exception>()
                        .WaitAndRetry(200, i => TimeSpan.FromSeconds(1));

                    await policy.Execute(() =>
                    {
                        var todayWeather = GetTodayWeather();
                        var result = BuildMessage(todayWeather);
                        return Bot.SendTextMessage(SettingsStore.Settings["chatId"], result.ToString());
                    });
                }

                await Task.Delay(TimeSpan.FromHours(1));
            }
        }

        private static StringBuilder BuildMessage(fact todayWeather)
        {
            var result = new StringBuilder();
            result.AppendLine($"������, ������ �� �������: {todayWeather.weather_type}");
            result.AppendLine($"�����������: {todayWeather.temperature.Value}");
            result.AppendLine($"���������: {todayWeather.humidity}");
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
