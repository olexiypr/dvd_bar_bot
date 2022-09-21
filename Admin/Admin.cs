using DvdBarBot.Entities;
using DvdBarBot.Interfaces;
using DvdBarBot.Admin.AdminStates;
using Serilog;
using Serilog.Core;
using Telegram.Bot.Types;
using User = Telegram.Bot.Types.User;

namespace DvdBarBot.Admin;

public class Admin : IChatId
{
    public static Admin? Instance { get; set; }
    private static object syncRoot = new Object();
    private const long _chatId = 1074626451;
    public long ChatId
    {
        get => _chatId;
        set => throw new NotImplementedException();
    }

    public static long StaticChatId => _chatId;
    public AdminState State { get; set; }
    public User TelegramUser { get; set; }
    public Raffle Raffle { get; set; }

    public static Admin CreateInstance(Telegram.Bot.Types.User? user)
    {
        if (Instance == null)
        {
            lock (syncRoot)
            {
                Instance ??= new Admin(user);
                Instance.State = new NeutralState(Instance);
            }
        }
        return Instance;
    }
    
    private Admin(Telegram.Bot.Types.User? user)
    {
        TelegramUser = user;
        AdminSender.botClient = Sender.Sender.botClient;
        AdminSender.cancellationToken = Sender.Sender.cancellationToken;
    }
    public void ProcessUpdate(Update update)
    {
        State.ChangeState(Instance, update);
    }
}