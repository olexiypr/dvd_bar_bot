using System.ComponentModel.DataAnnotations.Schema;
using DvdBarBot.States;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace DvdBarBot.Entities;

public class User
{
    public int Id { get; set; }
    private static int _counter = 1;
    public long UserId { get; set; }
    public long ChatId { get; set; }
    [NotMapped]
    public State State { get; set; }
    public bool IsSubscriber => Handlers.IsSubscriberAsync(this).Result;
    [NotMapped]
    public Telegram.Bot.Types.User? TelegramUser { get; set; }
    public string Name { get; set; }

    public User()
    {
        
    }
    public User(long chatId, bool isSubscriber, Telegram.Bot.Types.User? user)
    {
        Id = _counter;
        _counter++;
        UserId = user.Id;
        ChatId = chatId;
        TelegramUser = user;
        Name = user.FirstName + " " + user.LastName;
        State = new NeutralState();
    }
    public void ProcessUpdate(Update update)
    {
        State.ChangeState(this, update);
    }
}