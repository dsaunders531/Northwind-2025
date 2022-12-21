// <copyright file="StartAndEndDate.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns
{
    /// <summary>
    /// Try to capture all the sort orders in one object using binary compare.
    /// </summary>
    /// <remarks>There might be loads of similar sort orders which 
    /// can be grouped using binary comparison and flags (eg: SortOrder.HasFlag(SortOrder.Ascending).</remarks>
    public enum SortBy
    {
        Ascending = 1,
        Descending = 2,
        Name = 4,
        Price = 8,
        Popularity = 16,
        Product = 32,
        Category = 64,
        ProductNameAscending = SortBy.Product | SortBy.Name | SortBy.Ascending,
        ProductNameDescending = SortBy.Product | SortBy.Name | SortBy.Descending,
        ProductPriceAscending = SortBy.Product | SortBy.Price | SortBy.Ascending,
        ProductPriceDescending = SortBy.Product | SortBy.Price | SortBy.Descending,
        CategoryNameAscending = SortBy.Category | SortBy.Name | SortBy.Ascending,
        CategoryNameDescending = SortBy.Category | SortBy.Name | SortBy.Descending,
    }
}
