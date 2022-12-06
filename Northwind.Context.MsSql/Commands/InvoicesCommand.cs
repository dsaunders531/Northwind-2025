// <copyright file="InvoicesCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class InvoicesCommand : SqlRunnerCommandWithoutUndo<IList<Invoice>>
    {
        public InvoicesCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Invoices];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<Invoice>> RunCommand(SqlCommand com)
        {
            List<Invoice> result = new List<Invoice>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Invoice()
                        {
                            ShipName = reader["ShipName"]?.ToString() ?? string.Empty,
                            ShipAddress = reader["ShipAddress"]?.ToString() ?? string.Empty,
                            ShipCity = reader["ShipCity"]?.ToString() ?? string.Empty,
                            ShipRegion = reader["ShipRegion"].ToString() ?? string.Empty,
                            ShipPostalCode = reader["ShipPostalCode"]?.ToString() ?? string.Empty,
                            ShipCountry = reader["ShipCountry"]?.ToString() ?? string.Empty,
                            CustomerId = reader["CustomerID"]?.ToString() ?? string.Empty,
                            CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                            Address = reader["Address"]?.ToString() ?? string.Empty,
                            City = reader["City"]?.ToString() ?? string.Empty,
                            Region = reader["Region"]?.ToString() ?? string.Empty,
                            PostalCode = reader["PostalCode"]?.ToString() ?? string.Empty,
                            Country = reader["Country"]?.ToString() ?? string.Empty,
                            Salesperson = reader["Salesperson"]?.ToString() ?? string.Empty,
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(reader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                            ShipperName = reader["ShipperName"]?.ToString() ?? string.Empty,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Quantity = Convert.ToInt16(reader["Quantity"]),
                            Discount = Convert.ToSingle(reader["Discount"]),
                            ExtendedPrice = Convert.ToDecimal(reader["ExtendedPrice"]),
                            Freight = Convert.ToDecimal(reader["Freight"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
