// <copyright file="StartAndEndDate.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace Patterns
{
    public interface IPagedResponse<T>
        where T : class
    {
        int TotalItems { get; }
        int TotalPages { get; }
        int ItemsPerPage { get; }
        int CurrentPage { get; }
        SortBy SortOrder { get; }
        string SearchTerm { get; }
        T[] Page { get; }
    }

    /// <summary>
    /// Create a paged response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
            this.ItemsPerPage = itemsPerPage;
            this.CurrentPage = currentPage;
            this.SortOrder = sort;
            this.SearchTerm = searchTerm ?? string.Empty;
            this.Page = values;
        }

        public static IPagedResponse<TItem> Create<TItem>(int totalItems, int totalPages, int itemsPerPage, 
                                                            int currentPage, SortBy sort, string searchTerm, TItem[] values)
            where TItem : class
        {
            return new PagedResponse<TItem>(totalItems, totalPages, itemsPerPage, currentPage, sort, searchTerm, values);
        }

        public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }

        public int ItemsPerPage { get; private set; }

        public int CurrentPage { get; private set; }

        public SortBy SortOrder { get; private set; }

        public string SearchTerm { get; private set; }

        public T[] Page { get; private set; }
    }
}
