// <copyright file="PagedResponse{T}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

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
}
