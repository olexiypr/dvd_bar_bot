using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IAddUser
{
    Task Add(User user);
}