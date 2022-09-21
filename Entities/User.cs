using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Timers;
using DvdBarBot.Interfaces;
using DvdBarBot.States;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Timer = System.Timers.Timer;

namespace DvdBarBot.Entities;

public class User : IChatId
{
    public int Id { get; set; }
    private static int _counter = 1;
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public State State { get; set; }
    public bool IsSubscriber => Handlers.IsSubscriberAsync(this).Result;
    public Telegram.Bot.Types.User? TelegramUser { get; set; }
    public string Name { get; set; }
    public Raffle? Raffle { get; set; }
    public Message? SentMessage { get; set; }

    public User()
    {
        
    }
    public User(long chatId, Telegram.Bot.Types.User? user)
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

    public string GetInfo()
    {
        return $"Id: {Id}| user id: {UserId}| chat id: {ChatId}| state = {State}";
    }
}