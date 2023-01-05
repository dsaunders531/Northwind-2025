// <copyright file="Category.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Interfaces
{
    public interface ICategory
    {
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        string? Description { get; set; }
    }
}