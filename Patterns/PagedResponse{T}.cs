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

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public SortBy SortOrder { get; set; }

        public string SearchTerm { get; set; }

        public T[] Page { get; set; }

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
