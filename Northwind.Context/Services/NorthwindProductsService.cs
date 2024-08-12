using Microsoft.EntityFrameworkCore;
using Northwind.Context.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Northwind.Context.Models.Database;
using Patterns;
using Patterns.Extensions;

namespace Northwind.Context.Services
{
    public class NorthwindProductsService : INorthwindProductsService
    {
        public NorthwindProductsService(NorthwindContext context)
        {
            Context = context;
        }

        private NorthwindContext Context { get; set; }

        private const int ItemsPerPage = 10;

        private int TotalPages(int totalItems)
        {
            return (totalItems / ItemsPerPage) + (totalItems % ItemsPerPage != 0 ? 1 : 0);
        }

        private void AdjustPageToWithinMinMaxRange(ref int page, int totalPages)
        {
            if (page > totalPages)
            {
                page = totalPages;
            }
            else if (page < 1)
            {
                page = 1;
            }
        }

        public Task<IPagedResponse<CategoryApi>> GetCategories(int page, SortBy sort)
        {
            int totalItems = Context.Categories.Count();
            int totalPages = TotalPages(totalItems);

            AdjustPageToWithinMinMaxRange(ref page, totalPages);

            // Sort rule
            bool ascending = sort.IsAscending();

            IOrderedQueryable<Category> sortedValues;

            if (sort.IsAscending())
            {
                sortedValues = Context.Categories.OrderBy(o => o.CategoryName);
            }
            else
            {
                sortedValues = Context.Categories.OrderByDescending(o => o.CategoryName);
            }

            CategoryApi[] data = sortedValues
                                    .Skip((page - 1) * ItemsPerPage)
                                    .Take(ItemsPerPage)
                                    .Select(s => CategoryApi.Create(s))
                                    .ToArray() ?? Array.Empty<CategoryApi>();

            return Task.FromResult(PagedResponse<CategoryApi>.Create(totalItems, totalPages, ItemsPerPage, page, sort, string.Empty, data));
        }

        public async Task<ProductApi?> GetProductById(int productId)
        {
            Product? result = await Context.Products.FindAsync(productId);

            return result == default ? default : ProductApi.Create(result);
        }

        public Task<IPagedResponse<ProductApi>> GetProducts(int page, SortBy sort, string searchTerm)
        {
            IQueryable<Product> foundProducts;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                foundProducts = Context.Products;
            }
            else
            {
                foundProducts = Context.Products
                                            .Where(w => w.ProductName.ToLowerInvariant()
                                            .Contains(searchTerm.ToLowerInvariant())).AsQueryable();

            }

            int totalItems = foundProducts.Count();
            int totalPages = TotalPages(totalItems);

            AdjustPageToWithinMinMaxRange(ref page, totalPages);

            // sort
            bool ascending = sort.IsAscending();

            IOrderedQueryable<Product> sortedValues;

            switch (sort.Simplified())
            {
                case SortBy.Name:
                    if (ascending)
                    {
                        sortedValues = foundProducts.OrderBy(o => o.ProductName);
                    }
                    else
                    {
                        sortedValues = foundProducts.OrderByDescending(o => o.ProductName);
                    }

                    break;
                case SortBy.Price:
                    if (ascending)
                    {
                        sortedValues = foundProducts.OrderBy(o => o.UnitPrice);
                    }
                    else
                    {
                        sortedValues = foundProducts.OrderByDescending(o => o.UnitPrice);
                    }

                    break;
                case SortBy.Popularity:
                default:
                    if (ascending)
                    {
                        sortedValues = foundProducts.Include(i => i.OrderDetails).OrderBy(o => o.OrderDetails.Count);
                    }
                    else
                    {
                        sortedValues = foundProducts.Include(i => i.OrderDetails).OrderByDescending(o => o.OrderDetails.Count);
                    }
                    break;
            }

            ProductApi[] data = sortedValues
                                    .Skip((page - 1) * ItemsPerPage)
                                    .Take(ItemsPerPage)
                                    .Select(s => ProductApi.Create(s))
                                    .ToArray() ?? Array.Empty<ProductApi>();

            return Task.FromResult(PagedResponse<ProductApi>.Create(totalItems, totalPages, ItemsPerPage, page, sort, searchTerm, data));
        }

        public Task<IPagedResponse<ProductApi>> GetProductsInCategory(int categoryId, int page, SortBy sort)
        {
            IQueryable<Product> allProductsInCategory = Context.Categories
                                                                    .Include(i => i.Products)
                                                                    .Where(w => w.CategoryId == categoryId)
                                                                    .SelectMany(p => p.Products).AsQueryable();
            int totalItems = allProductsInCategory.Count();
            int totalPages = TotalPages(totalItems);
            AdjustPageToWithinMinMaxRange(ref page, totalPages);

            // Sorting
            bool ascending = sort.IsAscending();

            IOrderedQueryable<Product> orderedList;

            switch (sort.Simplified())
            {
                case SortBy.Name:
                    if (ascending)
                    {
                        orderedList = allProductsInCategory.OrderBy(o => o.ProductName);
                    }
                    else
                    {
                        orderedList = allProductsInCategory.OrderByDescending(o => o.ProductName);
                    }

                    break;
                case SortBy.Price:
                default:
                    if (ascending)
                    {
                        orderedList = allProductsInCategory.OrderBy(o => o.UnitPrice ?? decimal.MaxValue);
                    }
                    else
                    {
                        orderedList = allProductsInCategory.OrderByDescending(o => o.UnitPrice ?? decimal.MaxValue);
                    }

                    break;
            }

            ProductApi[] data = orderedList
                                    .Skip((page - 1) * ItemsPerPage)
                                    .Take(ItemsPerPage)
                                    .Select(s => ProductApi.Create(s))
                                    .ToArray() ?? Array.Empty<ProductApi>();

            return Task.FromResult(PagedResponse<ProductApi>.Create(totalItems, totalPages, ItemsPerPage, page, sort, categoryId.ToString(), data));
        }

        public Task<string[]> SearchProducts(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Task.FromResult(Array.Empty<string>());
            }
            else
            {
                return Task.FromResult(Context.Products
                                            .Where(s => s.ProductName.ToLowerInvariant().Contains(term.ToLowerInvariant()))
                                            .Select(s => s.ProductName)
                                            .OrderBy(o => o)
                                            .ToArray() ?? Array.Empty<string>());
            }
        }

        public Task<CategoryApi> GetCategory(int categoryId)
        {
            return Task.FromResult(CategoryApi.Create(Context.Categories.FirstOrDefault(f => f.CategoryId == categoryId) ?? new Category()));
        }
    }
}
