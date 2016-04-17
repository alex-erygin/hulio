using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace huliobot
{
    public interface IBot
    {
        Task Start();
    }
}