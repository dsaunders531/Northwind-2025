// <copyright file="OrdersQueryCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class OrdersQueryCommand : SqlRunnerCommandWithoutUndo<IList<OrdersQry>>
    {
        public OrdersQueryCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Orders Qry];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<OrdersQry>> RunCommand(SqlCommand com)
        {
            List<OrdersQry> result = new List<OrdersQry>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new OrdersQry()
                        {
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            CustomerId = reader["CustomerID"]?.ToString() ?? string.Empty,
                            EmployeeId = Convert.ToInt32(reader["EmployeeID"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(reader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                            ShipVia = Convert.ToInt32(reader["ShipVia"]),
                            Freight = Convert.ToDecimal(reader["Freight"]),
                            ShipName = reader["ShipName"]?.ToString() ?? string.Empty,
                            ShipAddress = reader["ShipAddress"]?.ToString() ?? string.Empty,
                            ShipCity = reader["ShipCity"]?.ToString() ?? string.Empty,
                            ShipRegion = reader["ShipRegion"]?.ToString() ?? string.Empty,
                            ShipPostalCode = reader["ShipPostalCode"]?.ToString() ?? string.Empty,
                            ShipCountry = reader["ShipCountry"]?.ToString() ?? string.Empty,
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                            Address = reader["Address"]?.ToString() ?? string.Empty,
                            City = reader["City"]?.ToString() ?? string.Empty,
                            Region = reader["Region"]?.ToString() ?? string.Empty,
                            PostalCode = reader["PostalCode"]?.ToString() ?? string.Empty,
                            Country = reader["Country"]?.ToString() ?? string.Empty,
                        });
                    }
                }
            }

            return result;
        }
    }
}
