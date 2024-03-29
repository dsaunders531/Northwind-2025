﻿using Northwind.Context.Models.Database;

namespace Northwind.Context.Extensions
{
    public static class OrderDetailsExtensions
    {
        /// <summary>
        /// This formula is used often.
        /// </summary>
        /// <param name="orderdetail"></param>
        /// <returns></returns>
        public static decimal? ExtendedPrice(this OrderDetail orderdetail)
        {
            return Math.Round((orderdetail.UnitPrice * orderdetail.Quantity * (decimal)((1 - orderdetail.Discount) / 100)) * 100, 2);
        }
    }
}
