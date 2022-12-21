// <copyright file="IntegrationTests.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.Context.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Database;
using Northwind.Context.Models.Reporting;
using Northwind.Context.MsSql.Contexts;
using Northwind.Context.MsSql.Services;


namespace Northwind.Tests.Sql
{
    /// <summary>
    /// Test the sql commands and service (functions which do not use the EF context).
    /// </summary>
    /// <remarks>
    /// Since we are just implementing existing stored procs and views. 
    /// All these tests do is check that the functions and methods work without error.    
    /// </remarks>
    public class IntegrationTests : INorthwindService
    {
        /// <summary>
        /// Gets or sets the interface for the service is implemented in the test class, when it changes we have to update the tests too!
        /// </summary>
        protected INorthwindService NorthwindService { get; set; }

        /// <summary>
        /// Gets or sets the context to get values which are not in the service.
        /// </summary>
        protected NorthwindContext NorthwindContext { get; set; }

        [SetUp]
        public virtual void Setup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            string connection = "Data Source=LAPTOP10\\SQLEXPRESS;Initial Catalog=Northwind-2025;Integrated Security=True;MultipleActiveResultSets=true;";

            if (NorthwindService == default)
            {
                NorthwindService = new NorthwindServiceSql(connection);
            }

            if (NorthwindContext == default)
            {
                DbContextOptions<NorthwindContext> contextOptions = new DbContextOptionsBuilder<NorthwindContext>().UseSqlServer(connection).Options;

                NorthwindContext = new NorthwindContextSql(contextOptions);

                // Make sure the database and all migrations have been applied
                NorthwindContext.Database.EnsureCreated();
                NorthwindContext.Database.Migrate();
                NorthwindContext.BringUpToDate(DateTime.UtcNow);
            }
        }

        [TearDown]
        public virtual void Teardown()
        {
            //NorthwindContext.Dispose();
        }

        [Test]
        public async Task AlphabeticalListOfProductsTest()
        {
            IList<AlphabeticalListOfProduct> products = await AlphabeticalListOfProducts();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());
            Assert.That(products.Count >= 2);

            // is it in alphabetical order?
            char firstProductFirstChar = products.First().ProductName.ToLower().First();
            char lastProductFirstChar = products.Last().ProductName.ToLower().First();

            Assert.That(firstProductFirstChar < lastProductFirstChar);
        }

        public Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts()
        {
            return NorthwindService.AlphabeticalListOfProducts();
        }

        [Test]
        public async Task CategorySalesFor1997sTest()
        {
            IList<CategorySalesForYear> sales = await CategorySalesFor1997s();

            Assert.IsNotNull(sales);
            Assert.IsFalse(sales.Any());
            Assert.That(sales.Count.Equals(0)); // its an old report so there should be no data.
        }

        [Obsolete]
        public Task<IList<CategorySalesForYear>> CategorySalesFor1997s()
        {
            return NorthwindService.CategorySalesFor1997s();
        }

        [Test]
        public async Task CategorySalesForYear()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            IList<CategorySalesForYear> sales = await CategorySalesForYear(year);

            Assert.IsNotNull(sales);
            Assert.IsTrue(sales.Any());
            Assert.That(sales.Count >= 1);
        }

        public Task<IList<CategorySalesForYear>> CategorySalesForYear(int year)
        {
            return NorthwindService.CategorySalesForYear(year);
        }

        [Test]
        public async Task CurrentProductListsTest()
        {
            IList<CurrentProductList> products = await CurrentProductLists();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());
            Assert.That(products.Count >= 1);
        }

        public Task<IList<CurrentProductList>> CurrentProductLists()
        {
            return NorthwindService.CurrentProductLists();
        }

        [Test]
        public async Task CustomerAndSuppliersByCitiesTest()
        {
            IList<CustomerAndSuppliersByCity> customersByCities = await CustomerAndSuppliersByCities();

            Assert.IsNotNull(customersByCities);
            Assert.IsTrue(customersByCities.Any());
            Assert.That(customersByCities.Count >= 1);
        }

        public Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities()
        {
            return NorthwindService.CustomerAndSuppliersByCities();
        }

        [Test]
        public async Task InvoicesTest()
        {
            IList<Invoice> invoices = await Invoices();

            Assert.IsNotNull(invoices);
            Assert.IsTrue(invoices.Any());
            Assert.That(invoices.Count >= 1);
        }

        public Task<IList<Invoice>> Invoices()
        {
            return NorthwindService.Invoices();
        }

        [Test]
        public async Task OrderDetailsExtendedsTest()
        {
            IList<OrderDetailsExtended> orderDetails = await OrderDetailsExtendeds();

            Assert.IsNotNull(orderDetails);
            Assert.IsTrue(orderDetails.Any());
            Assert.That(orderDetails.Count >= 1);
        }

        public Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds()
        {
            return NorthwindService.OrderDetailsExtendeds();
        }

        [Test]
        public async Task OrderSubtotalsTest()
        {
            IList<OrderSubtotal> orderSubtotals = await OrderSubtotals();

            Assert.IsNotNull(orderSubtotals);
            Assert.IsTrue(orderSubtotals.Any());
            Assert.That(orderSubtotals.Count >= 1);
        }

        public Task<IList<OrderSubtotal>> OrderSubtotals()
        {
            return NorthwindService.OrderSubtotals();
        }

        [Test]
        public async Task OrdersQriesTest()
        {
            IList<OrdersQry> orders = await OrdersQries();

            Assert.IsNotNull(orders);
            Assert.IsTrue(orders.Any());
            Assert.That(orders.Count >= 1);
        }

        public Task<IList<OrdersQry>> OrdersQries()
        {
            return NorthwindService.OrdersQries();
        }

        [Test]
        public async Task ProductSalesFor1997sTest()
        {
            IList<ProductSalesForYear> productSales = await ProductSalesFor1997s();

            Assert.IsNotNull(productSales);
            Assert.IsFalse(productSales.Any());
            Assert.That(productSales.Count == 0); // its not 1997 anymore
        }

        [Obsolete]
        public Task<IList<ProductSalesForYear>> ProductSalesFor1997s()
        {
            return NorthwindService.ProductSalesFor1997s();
        }

        [Test]
        public async Task ProductSalesForYearTest()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            IList<ProductSalesForYear> productSales = await ProductSalesForYear(year);

            Assert.IsNotNull(productSales);
            Assert.IsTrue(productSales.Any());
            Assert.That(productSales.Count >= 1);
        }

        public Task<IList<ProductSalesForYear>> ProductSalesForYear(int year)
        {
            return NorthwindService.ProductSalesForYear(year);
        }

        [Test]
        public async Task ProductsByCategoriesTest()
        {
            IList<ProductsByCategory> categories = await ProductsByCategories();

            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Any());
            Assert.That(categories.Count >= 1);
        }

        public Task<IList<ProductsByCategory>> ProductsByCategories()
        {
            return NorthwindService.ProductsByCategories();
        }

        [Test]
        public async Task SalesTotalsByAmountsTest()
        {
            IList<SalesTotalsByAmount> salesTotals = await SalesTotalsByAmounts();

            Assert.IsNotNull(salesTotals);
            Assert.IsFalse(salesTotals.Any());
            Assert.That(salesTotals.Count == 0); // hard coded date
        }

        [Obsolete]
        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts()
        {
            return NorthwindService.SalesTotalsByAmounts();
        }

        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts(DateTime start, DateTime end)
        {
            return NorthwindService.SalesTotalsByAmounts(start, end);
        }

        [Test]
        public async Task SalesTotalsByAmountsWithDatesTest()
        {
            DateTime current = DateTime.UtcNow;

            DateTime start = new DateTime(current.Month == 1 ? current.Year - 1 : current.Year, 1, 1);
            DateTime end = start.AddYears(1).Date.AddTicks(-1);

            IList<SalesTotalsByAmount> salesTotals = await SalesTotalsByAmounts(start, end);

            Assert.IsNotNull(salesTotals);
            Assert.IsTrue(salesTotals.Any());
            Assert.That(salesTotals.Count >= 1);
        }

        [Test]
        public async Task SummaryOfSalesByQuartersTest()
        {
            IList<SummaryOfSalesByQuarter> salesByQuarters = await SummaryOfSalesByQuarters();

            Assert.IsNotNull(salesByQuarters);
            Assert.IsTrue(salesByQuarters.Any());
            Assert.That(salesByQuarters.Count >= 1);
        }

        [Obsolete]
        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters()
        {
            return NorthwindService.SummaryOfSalesByQuarters();
        }

        [Test]
        public async Task SummaryOfSalesByQuartersWithDatesTest()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;
            int quarter = currentDate.Month == 1 ? 4 : 2;
            
            IList<SummaryOfSalesByQuarter> salesByQuarters = await SummaryOfSalesByQuarters(year, quarter);

            Assert.IsNotNull(salesByQuarters);
            Assert.IsTrue(salesByQuarters.Any());
            Assert.That(salesByQuarters.Count >= 1);
        }

        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters(int year, int quarter)
        {
            return NorthwindService.SummaryOfSalesByQuarters(year, quarter);
        }

        [Test]
        public async Task SummaryOfSalesByYearsTest()
        {
            IList<SummaryOfSalesByYear> salesByYears = await SummaryOfSalesByYears();

            Assert.IsNotNull(salesByYears);
            Assert.IsTrue(salesByYears.Any());
            Assert.That(salesByYears.Count >= 1);
        }

        [Obsolete]
        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears()
        {
            return NorthwindService.SummaryOfSalesByYears();
        }

        [Test]
        public async Task SummaryOfSalesByYearTest()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            IList<SummaryOfSalesByYear> salesByYears = await SummaryOfSalesByYears(year);

            Assert.IsNotNull(salesByYears);
            Assert.IsTrue(salesByYears.Any());
            Assert.That(salesByYears.Count >= 1);
        }

        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears(int year)
        {
            return NorthwindService.SummaryOfSalesByYears(year);
        }

        [Test]
        public async Task ProductsAboveAveragePricesTest()
        {
            IList<ProductsAboveAveragePrice> productAboveAveragePrice = await ProductsAboveAveragePrices();

            Assert.IsNotNull(productAboveAveragePrice);
            Assert.IsTrue(productAboveAveragePrice.Any());
            Assert.That(productAboveAveragePrice.Count >= 1);
        }

        public Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices()
        {
            return NorthwindService.ProductsAboveAveragePrices();
        }

        [Test]
        public async Task TenMostExpensiveProductsTest()
        {
            IList<MostExpensiveProduct> mostExpensiveProducts = await TenMostExpensiveProducts();

            Assert.IsNotNull(mostExpensiveProducts);
            Assert.IsTrue(mostExpensiveProducts.Any());
            Assert.That(mostExpensiveProducts.Count == 10);
        }

        public Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts()
        {
            return NorthwindService.TenMostExpensiveProducts();
        }

        [Test]
        public async Task EmployeeSalesByCountriesTest()
        {
            DateTime currentDate = DateTime.UtcNow;
            DateTime start = currentDate.Month == 1 ? new DateTime(currentDate.Year - 1, 1, 1) : new DateTime(currentDate.Year, 1, 1);
            DateTime end = start.AddYears(1).Date.AddTicks(-1);

            IList<EmployeeSalesByCountry> employeeSales = await EmployeeSalesByCountries(start, end);

            Assert.IsNotNull(employeeSales);
            Assert.That(employeeSales.Any());
        }

        public Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            return NorthwindService.EmployeeSalesByCountries(fromDate, toDate);
        }

        [Test]
        public async Task SalesByCategoryReportTest()
        {
            DateTime current = DateTime.UtcNow;

            Category category = NorthwindContext.Categories.FirstOrDefault();

            Assert.IsNotNull(category);

            IList<SaleByCategoryReport> saleByCategories = await SalesByCategory(category.CategoryName, current.Month == 1 ? current.Year - 1 : current.Year);

            Assert.IsNotNull(saleByCategories);
            Assert.IsTrue(saleByCategories.Any());
            Assert.That(saleByCategories.Count >= 1);
        }

        public Task<IList<SaleByCategoryReport>> SalesByCategory(string categoryName, int year)
        {
            return NorthwindService.SalesByCategory(categoryName, year);
        }

        [Test]
        public async Task SalesByYearTest()
        {
            DateTime current = DateTime.UtcNow;
            int year = current.Month == 1 ? current.Year - 1 : current.Year;

            IList<SaleByYear> saleByYears = await SalesByYear(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

            Assert.IsNotNull(saleByYears);

            Assert.That(saleByYears.Count >= 1);
        }

        public Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            return NorthwindService.SalesByYear(fromDate, toDate);
        }

        [Test]
        public async Task CustomerOdersTest()
        {
            Customer? customer = NorthwindContext.Customers.FirstOrDefault();

            Assert.IsNotNull(customer);

            IList<CustomerOrders> customerOrders = await CustomerOders(customer.CustomerId);

            Assert.IsNotNull(customerOrders);

            Assert.IsTrue(customerOrders.Any());

            Assert.IsNotNull(customerOrders.Count >= 1);
        }

        public Task<IList<CustomerOrders>> CustomerOders(string customerId)
        {
            return NorthwindService.CustomerOders(customerId);
        }

        [Test]
        public async Task CustomerOrderDetailTest()
        {
            Order order = NorthwindContext.Orders.FirstOrDefault();

            Assert.IsNotNull(order);

            IList<CustomerOrderDetail> serviceValue = await CustomerOrderDetail(order.OrderId);

            Assert.IsTrue(serviceValue.Any());
        }

        public Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId)
        {
            return NorthwindService.CustomerOrderDetail(orderId);
        }

        [Test]
        public async Task CustomerOrderHistoryTest()
        {
            Customer customer = NorthwindContext.Customers.FirstOrDefault();

            Assert.IsNotNull(customer);

            IList<CustomerOrderHistory> serviceValue = await CustomerOrderHistory(customer.CustomerId);

            Assert.IsTrue(serviceValue.Any());
        }

        public Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId)
        {
            return NorthwindService.CustomerOrderHistory(customerId);
        }

        [Test]
        public async Task QuarterlyOrdersTest()
        {
            IList<QuarterlyOrder> serviceValue = await QuarterlyOrders();

            Assert.IsNotNull(serviceValue);

            Assert.IsFalse(serviceValue.Any()); // hard-coded dates
        }

        [Obsolete]
        public Task<IList<QuarterlyOrder>> QuarterlyOrders()
        {
            return NorthwindService.QuarterlyOrders();
        }

        [Test]
        public async Task QuarterlyOrdersWithDatesTest()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;
            int quarter = currentDate.Month == 1 ? 4 : 2;

            IList<QuarterlyOrder> serviceValue = await QuarterlyOrders(year, quarter);

            Assert.IsNotNull(serviceValue);

            Assert.IsTrue(serviceValue.Any());
        }

        public Task<IList<QuarterlyOrder>> QuarterlyOrders(int year, int quarter)
        {
            return NorthwindService.QuarterlyOrders(year, quarter);
        }

        [Test]
        public async Task SalesByCategoryTest()
        {
            IList<SalesByCategory> serviceValue = await SalesByCategory();

            Assert.IsNotNull(serviceValue);

            Assert.IsFalse(serviceValue.Any()); // hard coded dates
        }

        [Obsolete]
        public Task<IList<SalesByCategory>> SalesByCategory()
        {
            return NorthwindService.SalesByCategory();
        }

        [Test]
        public async Task SalesByCategoryWithDatesTest()
        {
            DateTime currentDate = DateTime.UtcNow;

            int year = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;
            int quarter = currentDate.Month == 1 ? 4 : 2;

            IList<SalesByCategory> serviceValue = await SalesByCategories(year, quarter);

            Assert.IsNotNull(serviceValue);

            Assert.IsTrue(serviceValue.Any());
        }

        public Task<IList<SalesByCategory>> SalesByCategories(int year, int quarter)
        {
            return NorthwindService.SalesByCategories(year, quarter);
        }
    }
}