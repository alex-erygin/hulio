using System;
using System.Collections.Generic;

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
            RunMyBots(new IBot[] {hulio});
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