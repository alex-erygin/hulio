using Topshelf;

// ReSharper disable All

namespace huliobot
{
    /// <summary>
    ///     Бот по имени Хулио.
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<HulioBot>(s =>
                {
                    s.ConstructUsing(name => new HulioBot());
                    s.WhenStarted(bot => bot.Start());
                    s.WhenStopped(bot => bot.Stop());
                });

                x.StartAutomatically();
                x.SetDescription("Hulio bot");
                x.SetDisplayName("Hulio");
                x.SetServiceName("Hulio");
            });
        }
    }
}