// <copyright file="CustomerOrderDetail.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Reporting
{
    public class CustomerOrderDetail
    {
        public string? ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal ExtendedPrice { get; set; }
    }
}
