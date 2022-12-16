// <copyright file="CategorySalesFor1997.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Keyless]
    public class CategorySalesForYear
    {
        [StringLength(15)]
        public string CategoryName { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal? CategorySales { get; set; }
    }
}
