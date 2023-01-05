using Microsoft.AspNetCore.Mvc;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Definitions.Interfaces
{
    public interface ICategoriesController
    {
        Task<ActionResult<IPagedResponse<CategoryApi>>> Categories([FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending);
        Task<ActionResult<IPagedResponse<ProductApi>>> ProductsInCategories([FromRoute] int categoryId, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending);
    }
}