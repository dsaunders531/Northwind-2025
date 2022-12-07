// <copyright file="NorthwindServiceSql.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Northwind.Context.Interfaces;
using Northwind.Context.Models;
using Northwind.Context.MsSql.Commands;

namespace Northwind.Context.MsSql.Services
{
    /// <summary>
    /// A facade for the sql commands we want to run.
    /// </summary>
    public class NorthwindServiceSql : INorthwindService
    {
        public NorthwindServiceSql(string connection)
        {
            Connection = connection;
        }

        protected string Connection { get; set; }

        public Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts()
        {
            return new AlphabeticalListOfProductsCommand(Connection).Run();
        }

        public Task<IList<CategorySalesFor1997>> CategorySalesFor1997s()
        {
            return new CategorySalesFor1997Command(Connection).Run();
        }

        public Task<IList<CurrentProductList>> CurrentProductLists()
        {
            return new CurrentProductListCommand(Connection).Run();
        }

        public Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities()
        {
            return new CustomAndSuppliersByCitiesCommand(Connection).Run();
        }

        public Task<IList<CustomerOrders>> CustomerOders(string customerId)
        {
            return new CustomerOrdersCommand(Connection, customerId).Run();
        }

        public Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId)
        {
            return new CustomerOrderDetailCommand(Connection, orderId).Run();
        }

        public Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId)
        {
            return new CustomerOrderHistoryCommand(Connection, customerId).Run();
        }

        public Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            return new EmployeeSalesByCountriesCommand(Connection, new Patterns.StartAndEndDate() { StartDate = fromDate, EndDate = toDate }).Run();
        }

        public Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            return new SalesByYearCommand(Connection, new Patterns.StartAndEndDate() { StartDate = fromDate, EndDate = toDate }).Run();
        }

        public Task<IList<Invoice>> Invoices()
        {
            return new InvoicesCommand(Connection).Run();
        }

        public Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds()
        {
            return new OrderDetailsExtendedCommand(Connection).Run();
        }

        public Task<IList<OrdersQry>> OrdersQries()
        {
            return new OrdersQueryCommand(Connection).Run();
        }

        public Task<IList<OrderSubtotal>> OrderSubtotals()
        {
            return new OrderSubtotalsCommand(Connection).Run();
        }

        public Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices()
        {
            return new ProductsAboveAveragePriceCommand(Connection).Run();
        }

        public Task<IList<ProductSalesFor1997>> ProductSalesFor1997s()
        {
            return new ProductSalesFor1997Command(Connection).Run();
        }

        public Task<IList<ProductsByCategory>> ProductsByCategories()
        {
            return new ProductsByCategoryCommand(Connection).Run();
        }

        public Task<IList<SaleByCategoryReport>> SalesByCategory(string categoryName, int year)
        {
            return new SalesByCategoryCommand(Connection, new SalesByCateogryCommandParameters()
            {
                CategoryName = categoryName,
                Year = year,
            }).Run();
        }

        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts()
        {
            return new SalesTotalsByAmountCommand(Connection).Run();
        }

        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters()
        {
            return new SummaryOfSalesByQuarterCommand(Connection).Run();
        }

        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears()
        {
            return new SummaryOfSalesByYearCommand(Connection).Run();
        }

        public Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts()
        {
            return new TenMostExpensiveProductsCommand(Connection).Run();
        }
    }
}
