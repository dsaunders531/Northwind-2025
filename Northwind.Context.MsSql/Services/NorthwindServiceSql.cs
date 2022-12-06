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
            this.Connection = connection;
        }

        protected string Connection { get; set; }

        public Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts()
        {
            return new AlphabeticalListOfProductsCommand(this.Connection).Run();
        }

        public Task<IList<CategorySalesFor1997>> CategorySalesFor1997s()
        {
            return new CategorySalesFor1997Command(this.Connection).Run();
        }

        public Task<IList<CurrentProductList>> CurrentProductLists()
        {
            return new CurrentProductListCommand(this.Connection).Run();
        }

        public Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities()
        {
            return new CustomAndSuppliersByCitiesCommand(this.Connection).Run();
        }

        public Task<IList<CustomerOrders>> CustomerOders(string customerId)
        {
            return new CustomerOrdersCommand(this.Connection, customerId).Run();
        }

        public Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId)
        {
            return new CustomerOrderDetailCommand(this.Connection, orderId).Run();
        }

        public Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId)
        {
            return new CustomerOrderHistoryCommand(this.Connection, customerId).Run();
        }

        public Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            return new EmployeeSalesByCountries(this.Connection, new Patterns.StartAndEndDate() { StartDate = fromDate, EndDate = toDate }).Run();
        }

        public Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            return new SalesByYearCommand(this.Connection, new Patterns.StartAndEndDate() { StartDate = fromDate, EndDate = toDate }).Run();
        }

        public Task<IList<Invoice>> Invoices()
        {
            return new InvoicesCommand(this.Connection).Run();
        }

        public Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds()
        {
            return new OrderDetailsExtendedCommand(this.Connection).Run();
        }

        public Task<IList<OrdersQry>> OrdersQries()
        {
            return new OrdersQueryCommand(this.Connection).Run();
        }

        public Task<IList<OrderSubtotal>> OrderSubtotals()
        {
            return new OrderSubtotalsCommand(this.Connection).Run();
        }

        public Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices()
        {
            return new ProductsAboveAveragePriceCommand(this.Connection).Run();
        }

        public Task<IList<ProductSalesFor1997>> ProductSalesFor1997s()
        {
            return new ProductSalesFor1997Command(this.Connection).Run();
        }

        public Task<IList<ProductsByCategory>> ProductsByCategories()
        {
            return new ProductsByCategoryCommand(this.Connection).Run();
        }

        public Task<IList<SaleByCategoryReport>> SalesByCategory()
        {
            return new SalesByCategoryCommand(this.Connection).Run();
        }
       
        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts()
        {
            return new SalesTotalsByAmountCommand(this.Connection).Run();
        }

        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters()
        {
            return new SummaryOfSalesByQuarterCommand(this.Connection).Run();
        }

        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears()
        {
            return new SummaryOfSalesByYearCommand(this.Connection).Run();
        }

        public Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts()
        {
            return new TenMostExpensiveProductsCommand(this.Connection).Run();
        }        
    }
}
