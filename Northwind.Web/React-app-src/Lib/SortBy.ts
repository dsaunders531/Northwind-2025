
export const enum SortBy {
    Ascending = 1,
    Descending = 2,
    Name = 4,
    Price = 8,
    Popularity = 16,
    // the api requires every valid combination to be registered.
    // this may get complicated as the options increase.
    // however, by not including combinations - these will be rejected by any api calls.
    NameAscending = SortBy.Ascending | SortBy.Name,
    NameDescending = SortBy.Descending | SortBy.Name,
    PriceAscending = SortBy.Ascending | SortBy.Price,
    PriceDecending = SortBy.Descending | SortBy.Price,
    PopularityAscending = SortBy.Ascending | SortBy.Popularity,
    PopularityDescending = SortBy.Descending | SortBy.Popularity // 18
}
