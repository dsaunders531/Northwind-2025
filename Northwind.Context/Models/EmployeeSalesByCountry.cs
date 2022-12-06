// <copyright file="EmployeeSalesByCountry.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models
{
    public class EmployeeSalesByCountry
    {
        public string? Country { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public DateTime ShippedDate { get; set; }

        public int OrderId { get; set; }

        public decimal SaleAmount { get; set; }
    }
}
