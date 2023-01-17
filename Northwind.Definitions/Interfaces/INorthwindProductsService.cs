using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Context.Interfaces
{
    public interface INorthwindProductsService
    {
        Task<IPagedResponse<ProductApi>> GetProducts(int page, SortBy sort, string searchTerm);

        Task<ProductApi?> GetProductById(int productId);

        Task<string[]> SearchProducts(string term);

        Task<IPagedResponse<CategoryApi>> GetCategories(int page, SortBy sort);

        Task<IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, SortBy sort);

        Task<CategoryApi> GetCategory(int categoryId);
    }
}
