﻿// <copyright file="SaleByYear.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Reporting
{
    public class SaleByYear
    {
        public DateTime ShippedDate { get; set; }

        public int OrderId { get; set; }

        public decimal Subtotal { get; set; }

        public int Year { get; set; }
    }
}
