﻿// <copyright file="CustomerOrderHistory.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Northwind.Context.Models.Reporting
{
    public class CustomerOrderHistory
    {
        public string? ProductName { get; set; }

        public decimal Total { get; set; }
    }
}
