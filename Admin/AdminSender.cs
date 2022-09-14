using DvdBarBot.Menu;
using Telegram.Bot;

namespace DvdBarBot.Admin;

public static class AdminSender
{
    private static Admin Admin { get; set; }
    public static ITelegramBotClient botClient { get; set; }
    public static CancellationToken cancellationToken { get; set; }
    public static async Task SendMenuAsync()
    {
        await botClient.SendTextMessageAsync(chatId: Admin.ChatId,
            text: "Меню",
            replyMarkup: AdminMenu.GetMenu,
            cancellationToken: cancellationToken);
    }
}