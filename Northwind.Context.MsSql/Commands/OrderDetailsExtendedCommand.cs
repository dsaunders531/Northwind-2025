// <copyright file="OrderDetailsExtendedCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class OrderDetailsExtendedCommand : SqlRunnerCommandWithoutUndo<IList<OrderDetailsExtended>>
    {
        public OrderDetailsExtendedCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Order Details Extended];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<OrderDetailsExtended>> RunCommand(SqlCommand com)
        {
            List<OrderDetailsExtended> result = new List<OrderDetailsExtended>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new OrderDetailsExtended()
                        {
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Quantity = Convert.ToInt16(reader["Quantity"]),
                            Discount = Convert.ToSingle(reader["Discount"]),
                            ExtendedPrice = Convert.ToDecimal(reader["ExtendedPrice"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
