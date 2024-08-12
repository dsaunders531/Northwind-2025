// Interface for paged responses

import { SortBy } from "./SortBy"

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
    onSortChanged(sort: SortBy): void
    onSearchTermChanged(term: string): void
}

