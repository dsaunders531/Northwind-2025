// <copyright file="NorthwindService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Northwind.Context.Contexts;
using Northwind.Context.Extensions;
using Northwind.Context.Interfaces;
using Northwind.Context.Models;

namespace Northwind.Context.Services
{
    /// <summary>
    /// Implement the service using EF and Linq.
    /// </summary>
    /// <remarks>
    ///     It can be used with any NorthwindContext implementation - not just InMemory.
    ///     Performance optimisations can be made seperately using native stored procs, views or equivilant.
    /// </remarks>
    public class NorthwindService : INorthwindService
    {
        public NorthwindService(NorthwindContext context)
        {
            Context = context;
        }

        private NorthwindContext Context { get; set; }

        public Task<IList<AlphabeticalListOfProduct>> AlphabeticalListOfProducts()
        {
            return Task.FromResult(
                Context.Products
                .Where(d => d.Discontinued == false)                
                .Select(s => new AlphabeticalListOfProduct()
                {
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category == default ? string.Empty : s.Category.CategoryName,
                    Discontinued = s.Discontinued,
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    QuantityPerUnit = s.QuantityPerUnit,
                    ReorderLevel = s.ReorderLevel,
                    SupplierId = s.SupplierId,
                    UnitsInStock = s.UnitsInStock,
                    UnitPrice = s.UnitPrice,
                    UnitsOnOrder = s.UnitsOnOrder
                })                
                .OrderBy(y => y.ProductName)
                .ToList() as IList<AlphabeticalListOfProduct> ?? new List<AlphabeticalListOfProduct>());
        }

        public async Task<IList<CategorySalesFor1997>> CategorySalesFor1997s()
        {
            IList<ProductSalesFor1997> values = await ProductSalesFor1997s();

            if (values.Any())
            {
                // get the distinct categories
                return values.GroupBy(g => g.CategoryName)
                                            .Select(s => new CategorySalesFor1997()
                                            {
                                                CategoryName = s.Key,
                                                CategorySales = s.Sum(m => m.ProductSales)
                                            })
                                            .OrderByDescending(d => d.CategorySales)
                                                .ThenBy(y => y.CategoryName)
                                            .ToList() as IList<CategorySalesFor1997> ?? new List<CategorySalesFor1997>();
            }
            else
            {
                return new List<CategorySalesFor1997>();
            }            
        }

        public Task<IList<CurrentProductList>> CurrentProductLists()
        {
            return Task.FromResult(Context.Products
                                        .Where(w => w.Discontinued == false)
                                        .Select(s => new CurrentProductList() { ProductId = s.ProductId, ProductName = s.ProductName })
                                        .AsEnumerable()
                                        .GroupBy(y => y.ProductName)
                                        .ToList() as IList<CurrentProductList> ?? new List<CurrentProductList>());
        }

        public Task<IList<CustomerAndSuppliersByCity>> CustomerAndSuppliersByCities()
        {
            List<CustomerAndSuppliersByCity> result = new List<CustomerAndSuppliersByCity>();

            result.AddRange(Context.Customers.Select(s => new CustomerAndSuppliersByCity()
            {
                City = s.City,
                CompanyName = s.CompanyName,
                ContactName = s.ContactName,
                Relationship = "Customers"
            }));

            result.AddRange(Context.Suppliers.Select(s => new CustomerAndSuppliersByCity()
            {
                City = s.City,
                CompanyName = s.CompanyName,
                ContactName = s.ContactName,
                Relationship = "Suppliers"
            }));

            return Task.FromResult(result.OrderBy(o => o.City).ThenBy(b => b.CompanyName).ToList() as IList<CustomerAndSuppliersByCity>);
        }

        public Task<IList<CustomerOrders>> CustomerOders(string customerId)
        {
            return Task.FromResult(Context.Orders
                .Where(c => c.CustomerId == customerId)
                .Select(s => new CustomerOrders()
                {
                    OrderId = s.OrderId,
                    OrderDate = s.OrderDate ?? DateTime.UtcNow,
                    RequiredDate = s.RequiredDate ?? DateTime.UtcNow,
                    ShippedDate = s.ShippedDate ?? DateTime.UtcNow
                })
                .AsEnumerable()
                .OrderByDescending(r => r.RequiredDate)
                    .ThenByDescending(d => d.OrderDate)
                .ToList() as IList<CustomerOrders> ?? new List<CustomerOrders>());
        }

        public Task<IList<CustomerOrderDetail>> CustomerOrderDetail(int orderId)
        {
            IQueryable<OrderDetail> details = Context.OrderDetails.Where(w => w.OrderId == orderId);

            return Task.FromResult(Context.OrderDetails
                                        .Include(i => i.Product)
                                        .Where(w => w.OrderId == orderId)
                                        .Select(s => new CustomerOrderDetail()
                                        {
                                            Discount = (decimal)Math.Floor(s.Discount * 100),
                                            ExtendedPrice = s.ExtendedPrice() ?? 0,
                                            UnitPrice = Math.Round(s.UnitPrice, 2),
                                            Quantity = s.Quantity,
                                            ProductName = s.Product.ProductName
                                        })
                                        .AsEnumerable()
                                        .OrderBy(y => y.ProductName)
                                        .ToList() as IList<CustomerOrderDetail> ?? new List<CustomerOrderDetail>());
        }

        public Task<IList<CustomerOrderHistory>> CustomerOrderHistory(string customerId)
        {
            return Task.FromResult(Context.Orders
                                        .Include(i => i.OrderDetails)
                                        .ThenInclude(t => t.Product)
                                        .Where(w => w.CustomerId == customerId)
                                        .SelectMany(s => s.OrderDetails)
                                        .Select(s => new { s.Product.ProductName, s.Quantity })
                                        .GroupBy(g => g.ProductName)
                                        .Select(s => new CustomerOrderHistory()
                                        {
                                            ProductName = s.Key,
                                            Total = s.Sum(s => s.Quantity)
                                        })
                                        .AsEnumerable()
                                        .OrderBy(p => p.ProductName)
                                        .ToList() as IList<CustomerOrderHistory> ?? new List<CustomerOrderHistory>());
        }

        public Task<IList<EmployeeSalesByCountry>> EmployeeSalesByCountries(DateTime fromDate, DateTime toDate)
        {
            return Task.FromResult(Context.Orders
                                        .Include(i => i.Employee)
                                        .Include(o => o.OrderDetails)
                                        .Where(w => (w.ShippedDate.HasValue ? w.ShippedDate.Value.Date : DateTime.UtcNow.Date) >= fromDate.Date
                                           && (w.ShippedDate.HasValue ? w.ShippedDate.Value.Date : DateTime.UtcNow.Date) <= toDate.Date.AddDays(1).AddTicks(-1))
                                        .Select(s => new EmployeeSalesByCountry()
                                        {
                                            FirstName = s.Employee == default ? string.Empty : s.Employee.FirstName,
                                            LastName = s.Employee == default ? string.Empty : s.Employee.LastName,
                                            Country = s.Employee == default ? string.Empty : s.Employee.Country,
                                            OrderId = s.OrderId,
                                            ShippedDate = s.ShippedDate ?? DateTime.UtcNow,
                                            SaleAmount = s.OrderDetails.Sum(s => (decimal)(s.Quantity * (1 + s.Discount)) * s.UnitPrice)
                                        })
                                        .OrderBy(b => b.Country)
                                            .ThenBy(e => e.LastName)
                                            .ThenByDescending(y => y.SaleAmount)
                                        .ToList() as IList<EmployeeSalesByCountry> ?? new List<EmployeeSalesByCountry>());
        }

        public Task<IList<Invoice>> Invoices()
        {
            return Task.FromResult(Context.OrderDetails
                                        .Include(p => p.Product)
                                        .Include(i => i.Order)
                                            .ThenInclude(e => e.Employee)
                                        .Include(i => i.Order)
                                            .ThenInclude(c => c.Customer)
                                        .Include(i => i.Order)
                                            .ThenInclude(s => s.ShipViaNavigation)
                                        .Select(s => new Invoice()
                                        {
                                            ShipName = s.Order.ShipName,
                                            ShipAddress = s.Order.ShipAddress,
                                            ShipCity = s.Order.ShipCity,
                                            ShipRegion = s.Order.ShipRegion,
                                            ShipPostalCode = s.Order.ShipPostalCode,
                                            ShipCountry = s.Order.ShipCountry,
                                            CustomerId = s.Order.CustomerId,
                                            CustomerName = (s.Order.Customer ?? new Customer()).CompanyName,
                                            Address = (s.Order.Customer ?? new Customer()).Address,
                                            City = (s.Order.Customer ?? new Customer()).City,
                                            Region = (s.Order.Customer ?? new Customer()).Region,
                                            PostalCode = (s.Order.Customer ?? new Customer()).PostalCode,
                                            Country = (s.Order.Customer ?? new Customer()).Country,
                                            Salesperson = (s.Order.Employee ?? new Employee()).FullName(),
                                            OrderId = s.Order.OrderId,
                                            OrderDate = s.Order.OrderDate,
                                            RequiredDate = s.Order.RequiredDate,
                                            ShippedDate = s.Order.ShippedDate,
                                            ShipperName = (s.Order.ShipViaNavigation ?? new Shipper()).CompanyName,
                                            ProductId = s.Product.ProductId,
                                            ProductName = s.Product.ProductName,
                                            UnitPrice = s.UnitPrice,
                                            Quantity = s.Quantity,
                                            Discount = s.Discount,
                                            ExtendedPrice = s.ExtendedPrice(),
                                            Freight = s.Order.Freight
                                        })
                                        .OrderBy(y => y.OrderId)
                                            .ThenByDescending(p => p.ProductName)
                                        .ToList() as IList<Invoice> ?? new List<Invoice>());
        }

        public Task<IList<OrderDetailsExtended>> OrderDetailsExtendeds()
        {
            return Task.FromResult(
                Context.OrderDetails
                    .Include(p => p.Product)
                    .Select(s => new OrderDetailsExtended()
                    {
                        OrderId = s.OrderId,
                        ProductId = s.ProductId,
                        ProductName = s.Product.ProductName,
                        UnitPrice = s.UnitPrice,
                        Quantity = s.Quantity,
                        Discount = s.Discount,
                        ExtendedPrice = s.ExtendedPrice()
                    })
                    .OrderBy(p => p.ProductName)
                    .ToList() as IList<OrderDetailsExtended> ?? new List<OrderDetailsExtended>());
        }

        public Task<IList<OrdersQry>> OrdersQries()
        {
            return Task.FromResult(Context.Orders
                                        .Include(c => c.Customer)
                                        .Select(s => new OrdersQry()
                                        {
                                            OrderId = s.OrderId,
                                            ShipAddress = s.ShipAddress,
                                            ShipCity = s.ShipCity,
                                            EmployeeId = s.EmployeeId,
                                            ShipCountry = s.ShipCountry,
                                            ShipName = s.ShipName,
                                            ShippedDate = s.ShippedDate,
                                            ShipPostalCode = s.ShipPostalCode,
                                            ShipRegion = s.ShipRegion,
                                            ShipVia = s.ShipVia,
                                            Address = (s.Customer ?? new Customer()).Address,
                                            City = (s.Customer ?? new Customer()).City,
                                            CompanyName = (s.Customer ?? new Customer()).CompanyName,
                                            Country = (s.Customer ?? new Customer()).Country,
                                            CustomerId = s.CustomerId,
                                            Freight = s.Freight,
                                            OrderDate = s.OrderDate,
                                            PostalCode = (s.Customer ?? new Customer()).PostalCode,
                                            Region = (s.Customer ?? new Customer()).Region,
                                            RequiredDate = s.RequiredDate
                                        })
                                        .OrderBy(o => o.OrderDate)
                                        .ToList() as IList<OrdersQry> ?? new List<OrdersQry>());
        }

        public Task<IList<OrderSubtotal>> OrderSubtotals()
        {
            return Task.FromResult(Context.OrderDetails                                        
                                        .Select(s => new OrderSubtotal()
                                        {
                                            OrderId = s.OrderId,
                                            Subtotal = s.ExtendedPrice()
                                        })
                                        .AsEnumerable()
                                        .GroupBy(g => g.OrderId)                                        
                                        .AsEnumerable()
                                        .Select(s => new OrderSubtotal() { 
                                            OrderId = s.Key,
                                            Subtotal = s.Sum(u => u.Subtotal)
                                        })
                                        .OrderByDescending(d => d.Subtotal)
                                        .ToList() as IList<OrderSubtotal> ?? new List<OrderSubtotal>());
        }

        public Task<IList<ProductsAboveAveragePrice>> ProductsAboveAveragePrices()
        {            
            if (Context.Products?.Any() ?? false)
            {
                decimal averagePrice = Context.Products.Average(p => p.UnitPrice) ?? 0m;

                return Task.FromResult(Context.Products
                                            .Where(p => (p.UnitPrice ?? 0) > averagePrice)
                                            .Select(s => new ProductsAboveAveragePrice()
                                            {
                                                ProductName = s.ProductName,
                                                UnitPrice = s.UnitPrice
                                            })
                                            .OrderByDescending(b => b.UnitPrice)
                                            .ToList() as IList<ProductsAboveAveragePrice> ?? new List<ProductsAboveAveragePrice>());
            }
            else
            {
                return Task.FromResult(new List<ProductsAboveAveragePrice>() as IList<ProductsAboveAveragePrice>);
            }            
        }

        public Task<IList<ProductSalesFor1997>> ProductSalesFor1997s()
        {
            DateTime start = new DateTime(1997, 1, 1).Date;
            DateTime end = new DateTime(1998, 1, 1).Date.AddTicks(-1);

            return ProductSales(start, end);
        }

        public Task<IList<ProductSalesFor1997>> ProductSales(DateTime start, DateTime end)
        {
            return Task.FromResult(Context.OrderDetails
                                            .Include(o => o.Order)
                                            .Include(p => p.Product)
                                                .ThenInclude(c => c.Category)
                                            .Where(w => w.Order.ShippedDate >= start
                                                                && w.Order.ShippedDate <= end)
                                            .Select(s => new ProductSalesFor1997()
                                            {
                                                ProductName = s.Product.ProductName,
                                                CategoryName = (s.Product.Category ?? new Category()).CategoryName,
                                                ProductSales = s.ExtendedPrice()
                                            })
                                            .AsEnumerable()
                                            .GroupBy(g => new { g.ProductName, g.CategoryName })
                                            .Select(s => new ProductSalesFor1997()
                                            {
                                                ProductName = s.Key.CategoryName,
                                                CategoryName = s.Key.ProductName,
                                                ProductSales = s.Sum(x => x.ProductSales)
                                            })
                                            .OrderBy(p => p.CategoryName)
                                                .ThenBy(c => c.ProductName)
                                            .ToList() as IList<ProductSalesFor1997> ?? new List<ProductSalesFor1997>());
        }

        public Task<IList<ProductsByCategory>> ProductsByCategories()
        {
            return Task.FromResult(Context.Products
                                                .Include(c => c.Category)
                                                .Where(w => w.Discontinued == false)
                                                .Select(s => new ProductsByCategory()
                                                {
                                                    CategoryName = (s.Category ?? new Category()).CategoryName,
                                                    ProductName = s.ProductName,
                                                    QuantityPerUnit = s.QuantityPerUnit,
                                                    UnitsInStock = s.UnitsInStock,
                                                    Discontinued = s.Discontinued
                                                })
                                                .AsEnumerable()
                                                .OrderBy(o => o.CategoryName)
                                                    .ThenBy(p => p.ProductName)
                                                .ToList() as IList<ProductsByCategory> ?? new List<ProductsByCategory>());
        }

        public Task<IList<QuarterlyOrder>> QuarterlyOrders()
        {
            DateTime start = new DateTime(1997, 1, 1).Date;
            DateTime end = new DateTime(1998, 1, 1).Date.AddTicks(-1);

            return QuarterlyOrders(start, end);
        }

        public Task<IList<QuarterlyOrder>> QuarterlyOrders(DateTime start, DateTime end)
        {
            return Task.FromResult(Context.Orders
                                            .Include(c => c.Customer)
                                            .Where(w => w.OrderDate >= start && w.OrderDate <= end)
                                            .Select(s => new QuarterlyOrder()
                                            {
                                                CustomerId = s.CustomerId,
                                                CompanyName = (s.Customer ?? new Customer()).CompanyName,
                                                City = (s.Customer ?? new Customer()).City,
                                                Country = (s.Customer ?? new Customer()).Country
                                            })
                                            .AsEnumerable()
                                            .OrderBy(c => c.CompanyName)
                                            .ToList() as IList<QuarterlyOrder> ?? new List<QuarterlyOrder>());
        }

        public Task<IList<SalesByCategory>> SalesByCategory()
        {
            DateTime start = new DateTime(1997, 1, 1).Date;
            DateTime end = new DateTime(1998, 1, 1).Date.AddTicks(-1);

            return SalesByCategory(start, end);
        }

        public Task<IList<SalesByCategory>> SalesByCategory(DateTime start, DateTime end)
        {
            return Task.FromResult(Context.OrderDetails
                                            .Include(o => o.Order)
                                            .Include(p => p.Product)
                                                    .ThenInclude(c => c.Category)
                                            .Where(w => w.Order.OrderDate >= start && w.Order.OrderDate <= end)
                                            .AsEnumerable()
                                            .GroupBy(g => new { g.Product.CategoryId, (g.Product.Category ?? new Category()).CategoryName, g.Product.ProductName })                                            
                                            .Select(s => new SalesByCategory()
                                            {
                                                CategoryId = s.Key.CategoryId ?? 0,
                                                CategoryName = s.Key.CategoryName,
                                                ProductName = s.Key.ProductName,
                                                ProductSales = s.Sum(x => x.ExtendedPrice())
                                            })
                                            .AsEnumerable()
                                            .OrderBy(c => c.CategoryName)
                                                .ThenByDescending(y => y.ProductSales)
                                                .ThenBy(e => e.ProductName)
                                            .ToList() as IList<SalesByCategory> ?? new List<SalesByCategory>());
        }

        public Task<IList<SaleByCategoryReport>> SalesByCategory(string categoryName, int year)
        {
            return Task.FromResult(Context.OrderDetails
                                                .Include(p => p.Product)
                                                    .ThenInclude(c => c.Category)
                                                .Include(o => o.Order)
                                                .Where(w => ((w.Product ?? new Product()).Category ?? new Category()).CategoryName == categoryName && ((w.Order ?? new Order()).OrderDate ?? DateTime.UtcNow).Year == year)
                                                .Select(s => new { s.Product.ProductName, extendedPrice = s.ExtendedPrice() ?? 0 })
                                                .GroupBy(g => g.ProductName)
                                                .Select(s => new SaleByCategoryReport()
                                                {
                                                    ProductName = s.Key,
                                                    TotalPurchased = s.Sum(u => u.extendedPrice)
                                                })
                                                .OrderByDescending(y => y.TotalPurchased)
                                                    .ThenBy(b => b.ProductName)
                                                .ToList() as IList<SaleByCategoryReport> ?? new List<SaleByCategoryReport>());
        }

        public Task<IList<SaleByYear>> SalesByYear(DateTime fromDate, DateTime toDate)
        {
            return Task.FromResult(
                    Context.OrderDetails
                        .Include(o => o.Order)
                    .Where(w => w.Order.ShippedDate >= fromDate && w.Order.ShippedDate <= toDate)
                    .AsEnumerable()
                    .Select(s => new SaleByYear() { ShippedDate = s.Order.ShippedDate ?? DateTime.UtcNow, OrderId = s.OrderId, Subtotal = s.ExtendedPrice() ?? 0 })
                    .GroupBy(g => g.OrderId)
                    .Select(s => new SaleByYear()
                    {
                        ShippedDate = s.Max(m => m.ShippedDate),
                        OrderId = s.Key,
                        Subtotal = s.Sum(u => u.Subtotal),
                        Year = s.Max(m => m.ShippedDate).Year
                    })
                    .AsEnumerable()
                    .OrderBy(d => d.ShippedDate)
                        .ThenByDescending(y => y.Subtotal)
                    .ToList() as IList<SaleByYear> ?? new List<SaleByYear>());
        }

        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts()
        {
            DateTime start = new DateTime(1997, 1, 1);
            DateTime end = new DateTime(1998, 1, 1).Date.AddTicks(-1);

            return SalesTotalsByAmounts(start, end);
        }

        public Task<IList<SalesTotalsByAmount>> SalesTotalsByAmounts(DateTime start, DateTime end)
        {
            return Task.FromResult(
                Context.OrderDetails
                    .Include(o => o.Order)
                        .ThenInclude(c => c.Customer)
                    .Where(w1 => w1.Order.ShippedDate >= start && w1.Order.ShippedDate <= end)
                    .Select(s1 => new SalesTotalsByAmount()
                    {
                        SaleAmount = s1.ExtendedPrice(),
                        OrderId = s1.OrderId,
                        CompanyName = (s1.Order.Customer ?? new Customer()).CompanyName,
                        ShippedDate = s1.Order.ShippedDate
                    })
                    .AsEnumerable()
                    .GroupBy(g => new { g.OrderId, g.ShippedDate, g.CompanyName })
                    .Select(s2 => new SalesTotalsByAmount()
                    {
                        SaleAmount = s2.Sum(u => u.SaleAmount),
                        OrderId = s2.Key.OrderId,
                        CompanyName = s2.Key.CompanyName,
                        ShippedDate = s2.Key.ShippedDate
                    })
                    .Where(w2 => (w2.SaleAmount ?? 0) > 2500m)
                    .AsEnumerable()
                    .OrderByDescending(b => b.SaleAmount)
                        .ThenBy(d => d.ShippedDate)
                    .ToList() as IList<SalesTotalsByAmount> ?? new List<SalesTotalsByAmount>());
        }

        public Task<IList<SummaryOfSalesByQuarter>> SummaryOfSalesByQuarters()
        {
            return Task.FromResult(
                Context.OrderDetails
                    .Include(o => o.Order)
                    .Where(w => w.Order.ShippedDate.HasValue)
                    .Select(s1 => new SummaryOfSalesByQuarter()
                    {
                        ShippedDate = s1.Order.ShippedDate,
                        Subtotal = s1.ExtendedPrice(),
                        OrderId = s1.OrderId
                    })
                    .AsEnumerable()
                    .GroupBy(g => new { g.OrderId, g.ShippedDate })
                    .Select(s2 => new SummaryOfSalesByQuarter()
                    {
                        ShippedDate = s2.Key.ShippedDate,
                        OrderId = s2.Key.OrderId,
                        Subtotal = s2.Sum(u => u.Subtotal)
                    })
                    .AsEnumerable()
                    .OrderBy(b => b.ShippedDate)
                        .ThenByDescending(y => y.Subtotal)
                    .ToList() as IList<SummaryOfSalesByQuarter> ?? new List<SummaryOfSalesByQuarter>());
        }

        public Task<IList<SummaryOfSalesByYear>> SummaryOfSalesByYears()
        {
            return Task.FromResult(
                Context.OrderDetails
                    .Include(o => o.Order)
                    .Where(w => w.Order.ShippedDate.HasValue)
                    .Select(s1 => new SummaryOfSalesByYear()
                    {
                        ShippedDate = s1.Order.ShippedDate,
                        Subtotal = s1.ExtendedPrice(),
                        OrderId = s1.OrderId
                    })
                    .AsEnumerable()
                    .GroupBy(g => new { g.OrderId, g.ShippedDate })
                    .Select(s2 => new SummaryOfSalesByYear()
                    {
                        ShippedDate = s2.Key.ShippedDate,
                        OrderId = s2.Key.OrderId,
                        Subtotal = s2.Sum(u => u.Subtotal)
                    })
                    .AsEnumerable()
                    .OrderBy(b => b.ShippedDate)
                        .ThenByDescending(y => y.Subtotal)
                    .ToList() as IList<SummaryOfSalesByYear> ?? new List<SummaryOfSalesByYear>());
        }

        public Task<IList<MostExpensiveProduct>> TenMostExpensiveProducts()
        {
            return Task.FromResult(Context.Products
                                        .OrderByDescending(p => p.UnitPrice)
                                        .Take(10)
                                        .Select(s => new MostExpensiveProduct() { Name = s.ProductName, UnitPrice = s.UnitPrice ?? 0 })
                                        .ToList() as IList<MostExpensiveProduct> ?? new List<MostExpensiveProduct>());
        }
    }
}
