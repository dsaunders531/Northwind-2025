using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Web.Pages
{
    public class CategoriesModel : PageModel
    {
        public CategoriesModel(ILogger<CategoriesModel> logger, INorthwindProductsService productsService)
        {
            Logger = logger;
            NorthwindProductsService = productsService;
        }

        private ILogger<CategoriesModel> Logger { get; set; }

        private INorthwindProductsService NorthwindProductsService { get; set; }

        [BindProperty]
        public IPagedResponse<CategoryApi>? Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int p = 1, SortBy s = SortBy.Name | SortBy.Ascending)
        {
            try
            {
                this.Categories = await NorthwindProductsService.GetCategories(p, s);

                return Page();
            }
            catch (Exception e)
            {
                Logger.LogError("Error getting categories", e);
                throw;
            }
        }
    }
}
