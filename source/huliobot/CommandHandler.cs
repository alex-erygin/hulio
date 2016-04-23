using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    public interface ICommandHandler
    {
        void Handle(Api botApi, Update update);
    }
}