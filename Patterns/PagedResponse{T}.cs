// <copyright file="PagedResponse{T}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns
{
    /// <summary>
    /// Create a paged response.
    /// </summary>
    /// <typeparam name="T">The type for the response.</typeparam>
    public class PagedResponse<T> : IPagedResponse<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class.
        /// </summary>
        public PagedResponse()
        {
            Page = Array.Empty<T>();
            SearchTerm = string.Empty;
        }

        private PagedResponse(int totalItems, int totalPages, int itemsPerPage, int currentPage, SortBy sort, string searchTerm, T[] values)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            ItemsPerPage = itemsPerPage;
            CurrentPage = currentPage;
            SortOrder = sort;
            SearchTerm = searchTerm ?? string.Empty;
            Page = values;
        }

        /// <inheritdoc/>
        public int TotalItems { get; set; }

        /// <inheritdoc/>
        public int TotalPages { get; set; }

        /// <inheritdoc/>
        public int ItemsPerPage { get; set; }

        /// <inheritdoc/>
        public int CurrentPage { get; set; }

        /// <inheritdoc/>
        public SortBy SortOrder { get; set; }

        /// <inheritdoc/>
        public string SearchTerm { get; set; }

        /// <inheritdoc/>
        public T[] Page { get; set; }

        /// <summary>
        /// Create a paged response object.
        /// </summary>
        /// <typeparam name="TItem">The type of list item.</typeparam>
        /// <param name="totalItems">The total quantity of items.</param>
        /// <param name="totalPages">The quantity of pages.</param>
        /// <param name="itemsPerPage">Items per page.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="sort">The sort parameter (optional).</param>
        /// <param name="searchTerm">The search term (optional).</param>
        /// <param name="values">The data items for the current page.</param>
        /// <returns>A paged response object representing a page of data.</returns>
        public static IPagedResponse<TItem> Create<TItem>(
                                                            int totalItems,
                                                            int totalPages,
                                                            int itemsPerPage,
                                                            int currentPage,
                                                            SortBy sort,
                                                            string searchTerm,
                                                            TItem[] values)
            where TItem : class
        {
            return new PagedResponse<TItem>(totalItems, totalPages, itemsPerPage, currentPage, sort, searchTerm, values);
        }
    }
}
