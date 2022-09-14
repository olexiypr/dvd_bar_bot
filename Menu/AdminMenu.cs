using Telegram.Bot.Types.ReplyMarkups;

namespace DvdBarBot.Menu;

public static class AdminMenu
{
    public static ReplyKeyboardMarkup GetMenu => new (new []
    {
        new KeyboardButton[]
        {
            "Додати товар", "Додати категорію"
        },
        new KeyboardButton[]
        {
            "Видалити товар", "Видалити категорію"
        },
        new KeyboardButton[]
        {
            "Почати роіграш", "Кількість користувачів"
        },
        new KeyboardButton[]
        {
            "Технічна підтримка"
        }
    })
    {
        ResizeKeyboard = true,
    };
}