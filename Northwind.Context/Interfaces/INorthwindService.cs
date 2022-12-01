using Northwind.Context.Models;

namespace Northwind.Context.Interfaces
{
    public interface INorthwindService
    {
        // Views
        IList<AlphabeticalListOfProduct> AlphabeticalListOfProducts();
        IList<CategorySalesFor1997> CategorySalesFor1997s();
        IList<CurrentProductList> CurrentProductLists();
        IList<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities();
        IList<Invoice> Invoices();
        IList<OrderDetailsExtended> OrderDetailsExtendeds();
        IList<OrderSubtotal> OrderSubtotals();
        IList<OrdersQry> OrdersQries();
        IList<ProductSalesFor1997> ProductSalesFor1997s();
        IList<ProductsByCategory> ProductsByCategories();
        IList<SalesTotalsByAmount> SalesTotalsByAmounts();
        IList<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters();
        IList<SummaryOfSalesByYear> SummaryOfSalesByYears();
        IList<ProductsAboveAveragePrice> ProductsAboveAveragePrices();

        // Stored procs
        public IList<MostExpensiveProduct> TenMostExpensiveProducts();
        public IList<EmployeeSalesByCountry> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate);
        public IList<SalesByCategory> SalesByCategory();
        public IList<SaleByYear> SalesByYear(DateTime fromDate, DateTime toDate);
        public IList<CustomerOrders> CustomerOders(string customerId);
        public IList<CustomerOrderDetail> CustomerOrderDetail(int orderId);
        public IList<CustomerOrderHistory> CustomerOrderHistory(string customerId);
    }
}