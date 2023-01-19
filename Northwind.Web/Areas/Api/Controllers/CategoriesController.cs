using Microsoft.AspNetCore.Mvc;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Northwind.Definitions.Interfaces;
using Patterns;

namespace Northwind.Web.Areas.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoriesController: ControllerBase, ICategoriesController
    {
        public CategoriesController(INorthwindProductsService service, ILogger<CategoriesController> logger)
        {
            Service = service;
            Logger = logger;
        }

        private INorthwindProductsService Service { get; set; }

        private ILogger<CategoriesController> Logger { get; set; }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<CategoryApi>))]
        public async Task<ActionResult<IPagedResponse<CategoryApi>>> Categories([FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending)
        {
            try
            {
                return new JsonResult(await Service.GetCategories(page, sort));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Categories. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{categoryId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryApi))]
        public async Task<ActionResult<CategoryApi>> Category([FromRoute] int categoryId)
        {
            try
            {
                return new JsonResult(await Service.GetCategory(categoryId));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Category. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{categoryId}/products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<ProductApi>))]
        public async Task<ActionResult<IPagedResponse<ProductApi>>> ProductsInCategories([FromRoute] int categoryId, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending)
        {
            try
            {
                return new JsonResult(await Service.GetProductsInCategory(categoryId, page, sort));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error in GET Products in Categories. {ex.Message}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
