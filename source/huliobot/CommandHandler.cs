using Telegram.Bot;
using Telegram.Bot.Types;

namespace huliobot
{
    public interface ICommandHandler
    {
        void Handle(TelegramBotClient botApi, Message message);
    }
}