using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Client;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Northwind.Definitions.Interfaces;
using Patterns;

namespace Northwind.Web.Areas.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductsController : ControllerBase, IProductsController
    {
        public ProductsController(INorthwindProductsService service, ILogger<ProductsController> logger)
        { 
            Service = service;
            Logger = logger;
        }

        private INorthwindProductsService Service { get; set; }

        private ILogger<ProductsController> Logger { get; set; }

        [HttpGet]
        [Route("{productId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductApi))]
        public async Task<ActionResult<ProductApi>> Product([FromRoute] int productId)
        {
            try
            {
                ProductApi? result = await Service.GetProductById(productId);

                if (result == default)
                {
                    return NotFound();
                }
                else
                {
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Product {productId}. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<ProductApi>))]
        public async Task<ActionResult<IPagedResponse<ProductApi>>> Products([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending)
        {
            try
            {
                return new JsonResult(await Service.GetProducts(page, sort, searchTerm ?? string.Empty));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Products. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("search")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
        public async Task<ActionResult<string[]>> Search([FromQuery] string term)
        {
            try
            {
                string[] result = await Service.SearchProducts(term);

                if (result == default)
                {
                    return NotFound();
                }
                else
                {
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Search {term}. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
