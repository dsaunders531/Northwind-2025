// <copyright file="SalesTotalsByAmount.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using ClosedXML.Attributes;
using FileHelpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models.Reporting
{
    [DelimitedRecord(",")]
    [Keyless]
    public partial class SalesTotalsByAmount
    {
        [XLColumn(Order = 4, Header = "Sale Amount")]
        [FieldOrder(4)]
        [FieldConverter(ConverterKind.Decimal,".")]
        [Column(TypeName = "money")]
        public decimal? SaleAmount { get; set; }

        [XLColumn(Order = 1, Header = "Order Id")]
        [FieldOrder(1)]
        [Column("OrderID")]
        public int OrderId { get; set; }

        [XLColumn(Order = 2, Header = "Company Name")]
        [FieldOrder(2)]
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;

        [XLColumn(Order = 3, Header = "Shipped Date")]
        [FieldOrder(3)]
        [FieldConverter(ConverterKind.Date, "dd-MMM-yyyy")]
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
    }
}
