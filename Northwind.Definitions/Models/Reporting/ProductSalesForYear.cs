﻿// <copyright file="ProductSalesFor1997.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models.Reporting
{
    [Keyless]
    public partial class ProductSalesForYear
    {
        [StringLength(15)]
        public string CategoryName { get; set; } = null!;

        [StringLength(40)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal? ProductSales { get; set; }
    }
}
