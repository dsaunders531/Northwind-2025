// Interface for paged responses

export type IPagedResponse<T> = {
    totalItems: number,
    totalPages: number,
    itemsPerPage: number,
    currentPage: number,
    searchTerm: string,
    sortOrder: SortBy,
    page: T[],
    // handle the currentPage changing
    onCurrentPageChanged(page: number): void
}

export const enum SortBy {
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