using HTTPUser.Models;
namespace HTTPUser.Services;

public interface IApiService
{
    Task<Category[]> GetCategoriesAsync();
    Task<Category> GetCategoryAsync(long id);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(long id, string newTitle);
    Task DeleteCategoryAsync(long id);
    Task<Product[]> GetProductsAsync(long categoryId);
    Task CreateProductAsync(long categoryId, Product product);
}
