// <copyright file="SaleByCategoryReport.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using ClosedXML.Attributes;
using FileHelpers;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Context.Models.Reporting
{
    [DelimitedRecord(",")]
    [Keyless]
    public class SaleByCategoryReport
    {
        [XLColumn(Order = 1, Header = "Product Name")]
        [FieldOrder(1)]
        public string? ProductName { get; set; }

        [XLColumn(Order = 2, Header = "Total Purchased")]
        [FieldOrder(2)]
        [FieldConverter(ConverterKind.Decimal,".")]
        public decimal TotalPurchased { get; set; }
    }
}
