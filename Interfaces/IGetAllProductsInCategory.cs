using DvdBarBot.Entities;

namespace DvdBarBot.Interfaces;

public interface IGetAllProductsInCategory
{
    Task<IEnumerable<Product>> GetProductsInCategoryAsync(ProductCategory category);
    Task<IEnumerable<Product>> GetProductsInCategoryAsync(int categoryId);
}