// <copyright file="SaleByCategoryReport.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Reporting
{
    public class SaleByCategoryReport
    {
        public string? ProductName { get; set; }

        public decimal TotalPurchased { get; set; }
    }
}
