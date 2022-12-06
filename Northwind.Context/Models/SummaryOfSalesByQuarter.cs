// <copyright file="SummaryOfSalesByQuarter.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Keyless]
    public partial class SummaryOfSalesByQuarter
    {
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }

        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}
