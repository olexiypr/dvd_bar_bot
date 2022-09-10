using DvdBarBot.DataBase;

namespace DvdBarBot.Interfaces;

public interface IDbContext
{
    ApplicationDbContext dbContext { get; set; }
}