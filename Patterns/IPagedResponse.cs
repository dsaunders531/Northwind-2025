// <copyright file="PagedResponse{T}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns
{
    public interface IPagedResponse
    {
        int TotalItems { get; }
        int TotalPages { get; }
        int ItemsPerPage { get; }
        int CurrentPage { get; }
        SortBy SortOrder { get; }
        string SearchTerm { get; } 
    }

    public interface IPagedResponse<T> : IPagedResponse
        where T : class
    {        
        T[] Page { get; }
    }
}
