// <copyright file="ProductsByCategory.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Context.Models.Reporting
{
    [Keyless]
    public partial class ProductsByCategory
    {
        [StringLength(15)]
        public string CategoryName { get; set; } = null!;

        [StringLength(40)]
        public string ProductName { get; set; } = null!;

        [StringLength(20)]
        public string? QuantityPerUnit { get; set; }

        public short? UnitsInStock { get; set; }

        public bool Discontinued { get; set; }
    }
}
