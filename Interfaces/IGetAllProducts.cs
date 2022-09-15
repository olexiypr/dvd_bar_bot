using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IGetAllProducts
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}