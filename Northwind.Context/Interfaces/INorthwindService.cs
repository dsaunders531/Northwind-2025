// <copyright file="INorthwindService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Northwind.Context.Models;

namespace Northwind.Context.Interfaces
{
    public interface INorthwindService
    {
        // Views
        Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts();

        [Obsolete("Relies on a hard-coded date")]
        Task<IList<CategorySalesForYear>> CategorySalesFor1997s();

        Task<IList<CategorySalesForYear>> CategorySalesForYear(int year);

        Task<IList<CurrentProductList>> CurrentProductLists();

        Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities();

        Task<IList<Invoice>> Invoices();

        Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds();

        Task<IList<OrderSubtotal>> OrderSubtotals();

        Task<IList<OrdersQry>> OrdersQries();

        [Obsolete("Relies on a hard-coded date")]
        Task<IList<ProductSalesForYear>> ProductSalesFor1997s();

        Task<IList<ProductSalesForYear>> ProductSalesForYear(int year);

        Task<IList<ProductsByCategory>> ProductsByCategories();

        [Obsolete("Relies on hard-coded date")]
        Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts();

        Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts(DateTime start, DateTime end);

        [Obsolete("Relies on hard-coded date")]
        Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters();

        Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters(int year, int quarter);
        
        [Obsolete("Relies on hard-coded date")]
        Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears();

        Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears(int year);

        Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices();

        [Obsolete("Relies on a hard-coded date")]
        Task<IList<QuarterlyOrder>> QuarterlyOrders();

        Task<IList<QuarterlyOrder>> QuarterlyOrders(int year, int quarter);

        [Obsolete("Relies on a hard-coded date")]
        Task<IList<SalesByCategory>> SalesByCategory();

        Task<IList<SalesByCategory>> SalesByCategories(int year, int quarter);

        // Stored procs
        Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts();

        Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate);

        Task<IList<SaleByCategoryReport>> SalesByCategory(string categoryName, int year);

        Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate);

        Task<IList<CustomerOrders>> CustomerOders(string customerId);

        Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId);

        Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId);
    }
}