using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Web.Pages
{
    public class CategoryModel : PageModel
    {
        public CategoryModel(ILogger<CategoriesModel> logger, INorthwindProductsService northwindProductsService)
        {
            Logger = logger;
            NorthwindProductsService = northwindProductsService;
        }

        private ILogger<CategoriesModel> Logger { get; set; }

        private INorthwindProductsService NorthwindProductsService { get; set; }

        [BindProperty]
        public IPagedResponse<ProductApi>? Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId, int? p = 1, SortBy? s = SortBy.Name | SortBy.Ascending)
        {
            try
            {
                this.Products = await NorthwindProductsService.GetProductsInCategory(categoryId, p.Value, s.Value);

                return Page();
            }
            catch (Exception e)
            {
                Logger.LogError("Error getting category", e);
                throw;
            }
        }
    }
}
