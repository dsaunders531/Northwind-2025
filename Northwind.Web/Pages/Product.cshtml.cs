using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;

namespace Northwind.Web.Pages
{
    public class ProductModel(ILogger<ProductModel> logger, INorthwindProductsService productsService) : PageModel
    {
        [BindProperty]
        public ProductApi? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int productId)
        {
            try
            {
                Product = await productsService.GetProductById(productId);

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
                logger.LogError(e, "Error getting product");
                throw;
            }

        }
    }
}
