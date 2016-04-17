using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace huliobot
{
    /// <summary>
    ///     Бот по имени Хулио.
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
            var hulio = new HulioBot();
            var weatherBot = new WeatherBot();
            RunMyBots(new IBot[] {hulio, weatherBot});
            Console.ReadKey();
        }

        private static void RunMyBots(IEnumerable<IBot> bots)
        {
            foreach (var bot in bots)
            {
                bot.Start();
            }
        }
    }
}