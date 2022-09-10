using Telegram.Bot.Types.ReplyMarkups;

namespace DvdBarBot.Menu;

public static class Menu
{
    public static ReplyKeyboardMarkup GetMenu => new (new []
    {
        new KeyboardButton[]
        {
            "Каталог", "Пропозиції та акції"
        },
        new KeyboardButton[]
        {
            "Відгуки"
        },
        new KeyboardButton[]
        {
           "Зв'язатися з менеджером"
        },
        new KeyboardButton[]
        {
            "Технічна підтримка"
        }
    })
    {
        ResizeKeyboard = true,
        OneTimeKeyboard = true
    };
}