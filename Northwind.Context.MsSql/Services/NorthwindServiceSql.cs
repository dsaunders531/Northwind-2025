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
            throw new NotImplementedException();
        }

        public Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Invoice>> Invoices()
        {
            throw new NotImplementedException();
        }

        public Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds()
        {
            throw new NotImplementedException();
        }

        public Task<IList<OrdersQry>> OrdersQries()
        {
            throw new NotImplementedException();
        }

        public Task<IList<OrderSubtotal>> OrderSubtotals()
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices()
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductSalesFor1997>> ProductSalesFor1997s()
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductsByCategory>> ProductsByCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IList<SalesByCategory>> SalesByCategory()
        {
            throw new NotImplementedException();
        }

        public Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts()
        {
            throw new NotImplementedException();
        }

        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters()
        {
            throw new NotImplementedException();
        }

        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears()
        {
            throw new NotImplementedException();
        }

        public Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts()
        {
            throw new NotImplementedException();
        }        
    }
}
