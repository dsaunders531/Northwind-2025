// <copyright file="20221202110903_StoredProcsAndViews.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind.Context.MsSql.Migrations
{
    /// <summary>
    /// If this is migration is run on an existing instance it will fail (all commands have already been run).
    /// Using a try catch to work around this.
    /// </summary>
    public partial class StoredProcsAndViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //try
            //{
            //    // This migration has been created manually to add the stored procs and views.
            //    migrationBuilder.Sql("create view [dbo].[Alphabetical list of products] AS " +
            //        "SELECT Products.*, Categories.CategoryName" +
            //        "FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID" +
            //        "WHERE (((Products.Discontinued)=0))");

            //    migrationBuilder.Sql("create procedure [dbo].[Ten Most Expensive Products] AS" +
            //        "SET ROWCOUNT 10" +
            //        "SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice" +
            //        "FROM Products" +
            //        "ORDER BY Products.UnitPrice DESC");

            //    migrationBuilder.Sql("CREATE PROCEDURE [dbo].[SalesByCategory]" +
            //        "@CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998'" +
            //        "AS" +
            //        "IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' " +
            //        "BEGIN" +
            //        "SELECT @OrdYear = '1998'" +
            //        "END" +
            //        "SELECT ProductName," +
            //        "TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0)" +
            //        "FROM [Order Details] OD, Orders O, Products P, Categories C" +
            //        "WHERE OD.OrderID = O.OrderID " +
            //        "AND OD.ProductID = P.ProductID " +
            //        "AND P.CategoryID = C.CategoryID" +
            //        "AND C.CategoryName = @CategoryName" +
            //        "AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear" +
            //        "GROUP BY ProductName" +
            //        "ORDER BY ProductName");

            //    migrationBuilder.Sql("create procedure [dbo].[Sales by Year] " +
            //        "@Beginning_Date DateTime, @Ending_Date DateTime AS" +
            //        "SELECT Orders.ShippedDate, Orders.OrderID, \"Order Subtotals\".Subtotal, DATENAME(yy,ShippedDate) AS Year" +
            //        "FROM Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID" +
            //        "WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date");

            //    migrationBuilder.Sql("create procedure [dbo].[Employee Sales by Country] " +
            //        "@Beginning_Date DateTime, @Ending_Date DateTime AS" +
            //        "SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, \"Order Subtotals\".Subtotal AS SaleAmount" +
            //        "FROM Employees INNER JOIN " +
            //        "(Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID) " +
            //        "ON Employees.EmployeeID = Orders.EmployeeID\r\nWHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date");

            //    migrationBuilder.Sql("CREATE PROCEDURE [dbo].[CustOrdersOrders] @CustomerID nchar(5)" +
            //        "AS " +
            //        "SELECT OrderID, OrderDate, RequiredDate, ShippedDate " +
            //        "FROM Orders WHERE CustomerID = @CustomerID ORDER BY OrderID");

            //    migrationBuilder.Sql("CREATE PROCEDURE [dbo].[CustOrdersDetail] @OrderID int" +
            //        "AS" +
            //        "SELECT ProductName, " +
            //        "UnitPrice=ROUND(Od.UnitPrice, 2), Quantity, Discount=CONVERT(int, Discount * 100), ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2) " +
            //        "FROM Products P, [Order Details] Od" +
            //        "WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID");

            //    migrationBuilder.Sql("CREATE PROCEDURE [dbo].[CustOrderHist] @CustomerID nchar(5) AS " +
            //        "SELECT ProductName, Total=SUM(Quantity) " +
            //        "FROM Products P, [Order Details] OD, Orders O, Customers C " +
            //        "WHERE C.CustomerID = @CustomerID AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID " +
            //        "GROUP BY ProductName");

            //    migrationBuilder.Sql("create view [dbo].[Summary of Sales by Year] AS " +
            //        "SELECT Orders.ShippedDate, Orders.OrderID, \"Order Subtotals\".Subtotal " +
            //        "FROM Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID" +
            //        "WHERE Orders.ShippedDate IS NOT NULL");

            //    migrationBuilder.Sql("create view [dbo].[Summary of Sales by Quarter] AS " +
            //        "SELECT Orders.ShippedDate, Orders.OrderID, \"Order Subtotals\".Subtotal " +
            //        "FROM Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID " +
            //        "WHERE Orders.ShippedDate IS NOT NULL");

            //    migrationBuilder.Sql("create view [dbo].[Sales Totals by Amount] AS" +
            //        "SELECT \"Order Subtotals\".Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate" +
            //        "FROM \tCustomers INNER JOIN " +
            //        "(Orders INNER JOIN \"Order Subtotals\" ON Orders.OrderID = \"Order Subtotals\".OrderID) " +
            //        "ON Customers.CustomerID = Orders.CustomerID " +
            //        "WHERE (\"Order Subtotals\".Subtotal >2500) AND (Orders.ShippedDate BETWEEN '19970101' And '19971231')");

            //    migrationBuilder.Sql("create view [dbo].[Sales by Category] AS " +
            //        "SELECT Categories.CategoryID, Categories.CategoryName, Products.ProductName, " +
            //        "Sum(\"Order Details Extended\".ExtendedPrice) AS ProductSales " +
            //        "FROM Categories INNER JOIN " +
            //        "(Products INNER JOIN (Orders INNER JOIN \"Order Details Extended\" ON Orders.OrderID = \"Order Details Extended\".OrderID) ON Products.ProductID = \"Order Details Extended\".ProductID) ON Categories.CategoryID = Products.CategoryID" +
            //        "WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'" +
            //        "GROUP BY Categories.CategoryID, Categories.CategoryName, Products.ProductName");

            //    migrationBuilder.Sql("create view [dbo].[Quarterly Orders] AS " +
            //        "SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country " +
            //        "FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID " +
            //        "WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'");

            //    migrationBuilder.Sql("create view [dbo].[Products by Category] AS " +
            //        "SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued " +
            //        "FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID " +
            //        "WHERE Products.Discontinued <> 1");

            //    migrationBuilder.Sql("create view [dbo].[Products Above Average Price] AS " +
            //        "SELECT Products.ProductName, Products.UnitPrice " +
            //        "FROM Products " +
            //        "WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products)");

            //    migrationBuilder.Sql("create view [dbo].[Product Sales for 1997] AS " +
            //        "SELECT Categories.CategoryName, Products.ProductName, " +
            //        "Sum(CONVERT(money,(\"Order Details\".UnitPrice*Quantity*(1-Discount)/100))*100) AS ProductSales " +
            //        "FROM (Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID) " +
            //        "INNER JOIN (Orders " +
            //        "INNER JOIN \"Order Details\" ON Orders.OrderID = \"Order Details\".OrderID) " +
            //        "ON Products.ProductID = \"Order Details\".ProductID " +
            //        "WHERE (((Orders.ShippedDate) Between '19970101' And '19971231')) " +
            //        "GROUP BY Categories.CategoryName, Products.ProductName");

            //    migrationBuilder.Sql("create view [dbo].[Orders Qry] AS " +
            //        "SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, " +
            //        "Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, " +
            //        "Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, " +
            //        "Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country " +
            //        "FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID");

            //    migrationBuilder.Sql("create view [dbo].[Order Subtotals] AS " +
            //        "SELECT \"Order Details\".OrderID, Sum(CONVERT(money,(\"Order Details\".UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal " +
            //        "FROM \"Order Details\" " +
            //        "GROUP BY \"Order Details\".OrderID");

            //    migrationBuilder.Sql("create view [dbo].[Order Details Extended] AS " +
            //        "SELECT \"Order Details\".OrderID, \"Order Details\".ProductID, Products.ProductName, \"Order Details\".UnitPrice, \"Order Details\".Quantity, \"Order Details\".Discount, " +
            //        "(CONVERT(money,(\"Order Details\".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice\r\nFROM Products INNER JOIN \"Order Details\" ON Products.ProductID = \"Order Details\".ProductID");

            //    migrationBuilder.Sql("create view [dbo].[Invoices] AS " +
            //        "SELECT Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, " +
            //        "Orders.ShipCountry, Orders.CustomerID, Customers.CompanyName AS CustomerName, Customers.Address, Customers.City, " +
            //        "Customers.Region, Customers.PostalCode, Customers.Country, " +
            //        "(FirstName + ' ' + LastName) AS Salesperson, " +
            //        "Orders.OrderID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Shippers.CompanyName As ShipperName, \"Order Details\".ProductID, Products.ProductName, \"Order Details\".UnitPrice, \"Order Details\".Quantity, \"Order Details\".Discount, " +
            //        "(CONVERT(money,(\"Order Details\".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice, Orders.Freight " +
            //        "FROM Shippers INNER JOIN " +
            //        "(Products INNER JOIN ((Employees INNER JOIN (Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) ON Employees.EmployeeID = Orders.EmployeeID) " +
            //        "INNER JOIN \"Order Details\" ON Orders.OrderID = \"Order Details\".OrderID) " +
            //        "ON Products.ProductID = \"Order Details\".ProductID) " +
            //        "ON Shippers.ShipperID = Orders.ShipVia");

            //    migrationBuilder.Sql("create view [dbo].[Customer and Suppliers by City] AS " +
            //        "SELECT City, CompanyName, ContactName, 'Customers' AS Relationship " +
            //        "FROM Customers " +
            //        "UNION SELECT City, CompanyName, ContactName, 'Suppliers' " +
            //        "FROM Suppliers");

            //    migrationBuilder.Sql("create view [dbo].[Current Product List] AS " +
            //        "SELECT Product_List.ProductID, Product_List.ProductName " +
            //        "FROM Products AS Product_List " +
            //        "WHERE (((Product_List.Discontinued)=0))");

            //    migrationBuilder.Sql("create view [dbo].[Category Sales for 1997] AS " +
            //        "SELECT \"Product Sales for 1997\".CategoryName, Sum(\"Product Sales for 1997\".ProductSales) AS CategorySales " +
            //        "FROM \"Product Sales for 1997\" " +
            //        "GROUP BY \"Product Sales for 1997\".CategoryName");
            //}
            //catch
            //{
            //    // do nothing
            //}
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop procedure [dbo].[Employee Sales by Country]");
            migrationBuilder.Sql("drop procedure [dbo].[Sales by Year]");
            migrationBuilder.Sql("drop procedure [dbo].[Ten Most Expensive Products]");
            migrationBuilder.Sql("drop view [dbo].[Category Sales for 1997]");
            migrationBuilder.Sql("drop view [dbo].[Sales by Category]");
            migrationBuilder.Sql("drop view [dbo].[Sales Totals by Amount]");
            migrationBuilder.Sql("drop view [dbo].[Summary of Sales by Quarter]");
            migrationBuilder.Sql("drop view [dbo].[Summary of Sales by Year]");
            migrationBuilder.Sql("drop view [dbo].[Invoices]");
            migrationBuilder.Sql("drop view [dbo].[Order Details Extended]");
            migrationBuilder.Sql("drop view [dbo].[Order Subtotals]");
            migrationBuilder.Sql("drop view [dbo].[Product Sales for 1997]");
            migrationBuilder.Sql("drop view [dbo].[Alphabetical list of products]");
            migrationBuilder.Sql("drop view [dbo].[Current Product List]");
            migrationBuilder.Sql("drop view [dbo].[Orders Qry]");
            migrationBuilder.Sql("drop view [dbo].[Products Above Average Price]");
            migrationBuilder.Sql("drop view [dbo].[Products by Category]");
            migrationBuilder.Sql("drop view [dbo].[Quarterly Orders]");
            migrationBuilder.Sql("drop view [dbo].[Customer and Suppliers by City]");
            migrationBuilder.Sql("drop procedure [dbo].[CustOrdersDetail]");
            migrationBuilder.Sql("drop procedure [dbo].[CustOrdersOrders]");
            migrationBuilder.Sql("drop procedure [dbo].[CustOrderHist]");
            migrationBuilder.Sql("drop procedure [dbo].[SalesByCategory]");
        }
    }
}
