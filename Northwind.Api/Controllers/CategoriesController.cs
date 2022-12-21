﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Api.Controllers
{
    /// <summary>
    /// Controller for all supported product category operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public CategoriesController(INorthwindProductsService service, ILogger<CategoriesController> logger)
        {
            this.Service = service;
            this.Logger = logger;
        }

        private INorthwindProductsService Service { get; set; }

        private ILogger<CategoriesController> Logger { get; set; }

        /// <summary>
        /// Get a list of categories.
        /// </summary>
        /// <param name="page">The page of categories you want to see.</param>
        /// <param name="sort">The sort order of the categories.</param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<CategoryApi>))]
        public async Task<ActionResult<IPagedResponse<CategoryApi>>> Categories([FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.Ascending | SortBy.Name)
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

        /// <summary>
        /// Get a list of products within a category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="page">The page of products within the category you want to see.</param>
        /// <param name="sort">The sort order for the products.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{categoryId}/products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<ProductApi>))]
        public async Task<ActionResult<IPagedResponse<ProductApi>>> ProductsInCategories([FromRoute] int categoryId, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.Ascending | SortBy.Name)
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
