// <copyright file="ProductsController.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Api;
using Northwind.Definitions.Interfaces;
using Patterns;

namespace Northwind.Api.Controllers
{
    /// <summary>
    /// Controller for all supported product operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase, IProductsController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ProductsController(INorthwindProductsService service, ILogger<ProductsController> logger)
        {
            Service = service;
            Logger = logger;
        }

        private INorthwindProductsService Service { get; set; }

        private ILogger<ProductsController> Logger { get; set; }

        /// <summary>
        /// Get a paged list of products.
        /// </summary>
        /// <param name="searchTerm">Optional search term to filter products.</param>
        /// <param name="page">The page of products you want to view.</param>
        /// <param name="sort">The order of products in the list.</param>
        /// <returns>A paged list of products.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IPagedResponse<ProductApi>))]
        public async Task<ActionResult<IPagedResponse<ProductApi>>> Products([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.Name | SortBy.Ascending)
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

        /// <summary>
        /// Get a product by product id.
        /// </summary>
        /// <param name="productId">The product id you want to see.</param>
        /// <returns>A product object.</returns>
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

        /// <summary>
        /// Search for product names.
        /// </summary>
        /// <param name="term">The search term you want to use.</param>
        /// <returns>A list of matching product names.</returns>
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
