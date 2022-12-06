﻿// <copyright file="ProductsAboveAveragePrice.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Keyless]
    public partial class ProductsAboveAveragePrice
    {
        [StringLength(40)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
    }
}
