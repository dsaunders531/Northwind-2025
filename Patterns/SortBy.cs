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
        // the api requires every valid combination to be registered.
        // this may get complicated as the options increase.
        // however, by not including combinations - these will be rejected by any api calls.
        NameAscending = SortBy.Ascending | SortBy.Name, // 5
        NameDescending = SortBy.Descending | SortBy.Name, // 6
        PriceAscending = SortBy.Ascending | SortBy.Price, // 9
        PriceDecending = SortBy.Descending | SortBy.Price, // 10
        PopularityAscending = SortBy.Ascending | SortBy.Popularity, // 17
        PopularityDescending = SortBy.Descending | SortBy.Popularity // 18
    }
}
