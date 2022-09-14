using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using DvdBarBot.Admin.AdminStates;
using Telegram.Bot.Types;
using User = Telegram.Bot.Types.User;

namespace DvdBarBot.Admin;

public class Admin
{
    private const long _chatId = 1074626451;
    public new static long ChatId => _chatId;
    public long UserId { get; set; }
    public AdminState State { get; set; }
    public User TelegramUser { get; set; }
    public Raffle Raffle { get; set; }
    
    public Admin(Telegram.Bot.Types.User? user)
    {
        TelegramUser = user;
        AdminSender.botClient = Sender.Sender.botClient;
        AdminSender.cancellationToken = Sender.Sender.cancellationToken;
        State = new NeutralState(this);
    }
    public void ProcessUpdate(Update update)
    {
        State.ChangeState(this, update);
    }
}