using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IAddUser
{
    Task AddUserAsync(User user);
}