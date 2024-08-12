// <copyright file="Product.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Interfaces
{
    public interface IProduct
    {
        int? CategoryId { get; set; }
        bool Discontinued { get; set; }
        int ProductId { get; set; }
        string ProductName { get; set; }
        string? QuantityPerUnit { get; set; }
        decimal? UnitPrice { get; set; }
        short? UnitsInStock { get; set; }
    }
}