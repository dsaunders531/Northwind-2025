// <copyright file="CurrentProductList.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Keyless]
    public class CurrentProductList
    {
        [Column("ProductID")]
        public int ProductId { get; set; }

        [StringLength(40)]
        public string ProductName { get; set; } = null!;
    }
}
