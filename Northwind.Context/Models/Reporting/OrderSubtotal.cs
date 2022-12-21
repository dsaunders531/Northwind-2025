// <copyright file="OrderSubtotal.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models.Reporting
{
    [Keyless]
    public partial class OrderSubtotal
    {
        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}
