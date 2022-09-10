using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IGetAllProductsInCategory
{
    IEnumerable<Product> GetProductsInCategory(ProductCategory category);
    IEnumerable<Product> GetProductsInCategory(int categoryId);
}