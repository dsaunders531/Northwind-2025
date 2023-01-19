// <copyright file="ProductsController.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Northwind.Context.Models.Api;
using Patterns;

namespace Northwind.Definitions.Interfaces
{
    public interface IProductsController
    {
        Task<ActionResult<ProductApi>> Product([FromRoute] int productId);
        Task<ActionResult<IPagedResponse<ProductApi>>> Products([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] SortBy sort = SortBy.NameAscending);
        Task<ActionResult<string[]>> Search([FromQuery] string term);
    }
}