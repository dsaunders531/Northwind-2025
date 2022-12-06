﻿namespace Northwind.Context.Models
{
    public class CustomerOrderDetail
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal ExtendedPrice { get; set; }
    }
}