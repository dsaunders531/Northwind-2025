using Northwind.Context.Models;

namespace Northwind.Context.Interfaces
{
    public interface INorthwindService
    {
        // Views
        Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts();
        Task<IList<CategorySalesFor1997>> CategorySalesFor1997s();
        Task<IList<CurrentProductList>> CurrentProductLists();
        Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities();
        Task<IList<Invoice>> Invoices();
        Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds();
        Task<IList<OrderSubtotal>> OrderSubtotals();
        Task<IList<OrdersQry>> OrdersQries();
        Task<IList<ProductSalesFor1997>> ProductSalesFor1997s();
        Task<IList<ProductsByCategory>> ProductsByCategories();
        Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts();
        Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters();
        Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears();
        Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices();

        // Stored procs
        Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts();
        Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate);
        Task<IList<SalesByCategory>> SalesByCategory();
        Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate);
        Task<IList<CustomerOrders>> CustomerOders(string customerId);
        Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId);
        Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId);
    }
}