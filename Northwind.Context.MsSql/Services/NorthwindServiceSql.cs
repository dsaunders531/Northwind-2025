using Northwind.Context.Interfaces;
using Northwind.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Context.MsSql.Services
{
    public class NorthwindServiceSql : INorthwindService
    {
        public IList<AlphabeticalListOfProduct> AlphabeticalListOfProducts()
        {
            throw new NotImplementedException();
        }

        public IList<CategorySalesFor1997> CategorySalesFor1997s()
        {
            throw new NotImplementedException();
        }

        public IList<CurrentProductList> CurrentProductLists()
        {
            throw new NotImplementedException();
        }

        public IList<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities()
        {
            throw new NotImplementedException();
        }

        public IList<CustomerOrders> CustomerOders(string customerId)
        {
            throw new NotImplementedException();
        }

        public IList<CustomerOrderDetail> CustomerOrderDetail(int orderId)
        {
            throw new NotImplementedException();
        }

        public IList<CustomerOrderHistory> CustomerOrderHistory(string customerId)
        {
            throw new NotImplementedException();
        }

        public IList<EmployeeSalesByCountry> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public IList<Invoice> Invoices()
        {
            throw new NotImplementedException();
        }

        public IList<OrderDetailsExtended> OrderDetailsExtendeds()
        {
            throw new NotImplementedException();
        }

        public IList<OrdersQry> OrdersQries()
        {
            throw new NotImplementedException();
        }

        public IList<OrderSubtotal> OrderSubtotals()
        {
            throw new NotImplementedException();
        }

        public IList<ProductsAboveAveragePrice> ProductsAboveAveragePrices()
        {
            throw new NotImplementedException();
        }

        public IList<ProductSalesFor1997> ProductSalesFor1997s()
        {
            throw new NotImplementedException();
        }

        public IList<ProductsByCategory> ProductsByCategories()
        {
            throw new NotImplementedException();
        }

        public IList<SalesByCategory> SalesByCategory()
        {
            throw new NotImplementedException();
        }

        public IList<SaleByYear> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public IList<SalesTotalsByAmount> SalesTotalsByAmounts()
        {
            throw new NotImplementedException();
        }

        public IList<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters()
        {
            throw new NotImplementedException();
        }

        public IList<SummaryOfSalesByYear> SummaryOfSalesByYears()
        {
            throw new NotImplementedException();
        }

        public IList<MostExpensiveProduct> TenMostExpensiveProducts()
        {
            throw new NotImplementedException();
        }
    }
}
