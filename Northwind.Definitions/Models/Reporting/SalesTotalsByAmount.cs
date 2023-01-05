// <copyright file="SalesTotalsByAmount.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models.Reporting
{
    [Keyless]
    public partial class SalesTotalsByAmount
    {
        [Column(TypeName = "money")]
        public decimal? SaleAmount { get; set; }

        [Column("OrderID")]
        public int OrderId { get; set; }

        [StringLength(40)]
        public string CompanyName { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
    }
}
