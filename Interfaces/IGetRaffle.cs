using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IGetRaffle
{
    Task<Raffle> GetRaffleAsync();
}