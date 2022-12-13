// <copyright file="SalesByCateogryReportCommandParameters.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByCateogryReportCommandParameters
    {
        public string? CategoryName { get; set; }

        public int Year { get; set; }
    }
}
