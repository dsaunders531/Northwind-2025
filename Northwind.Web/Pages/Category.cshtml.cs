using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Web.Pages
{
    public class CategoryModel(ILogger<CategoriesModel> logger, INorthwindProductsService northwindProductsService) : PageModel
    {
        [BindProperty]
        public IPagedResponse<ProductApi>? Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId, int? p = 1, SortBy? s = SortBy.Name | SortBy.Ascending)
        {
            try
            {
                Products = await northwindProductsService.GetProductsInCategory(categoryId,
                                                                                p ?? 1,
                                                                                s ?? SortBy.Name | SortBy.Ascending);

                return Page();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error getting category!");
                throw;
            }
        }
    }
}
