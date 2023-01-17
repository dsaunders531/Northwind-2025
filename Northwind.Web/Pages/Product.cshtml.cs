using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;

namespace Northwind.Web.Pages
{    
    public class ProductModel : PageModel
    {
        public ProductModel(ILogger<ProductModel> logger, INorthwindProductsService productsService)
        {
            Logger = logger;
            NorthwindProductsService = productsService;
        }

        private ILogger<ProductModel> Logger { get; set; }

        private INorthwindProductsService NorthwindProductsService { get; set; }

        [BindProperty]
        public ProductApi? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int productId)
        {
            try
            {
                this.Product = await NorthwindProductsService.GetProductById(productId);

                if (Product == default)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error getting product", e);
                throw;
            }

        }
    }
}
