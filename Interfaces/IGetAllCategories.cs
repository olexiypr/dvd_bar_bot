using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IGetAllCategories
{
    IEnumerable<ProductCategory> ProductCategories { get; }
}