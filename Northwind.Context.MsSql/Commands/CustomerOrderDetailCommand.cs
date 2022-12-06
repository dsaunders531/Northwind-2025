// <copyright file="CustomerOrderDetailCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class CustomerOrderDetailCommand : SqlRunnerCommandWithoutUndo<IList<CustomerOrderDetail>, int>
    {
        public CustomerOrderDetailCommand(string connection, int parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "CustOrdersDetail";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@OrderId", System.Data.SqlDbType.Int) { Value = Parameters });
        }

        protected override async Task<IList<CustomerOrderDetail>> RunCommand(SqlCommand com)
        {
            List<CustomerOrderDetail> result = new List<CustomerOrderDetail>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CustomerOrderDetail()
                        {
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            ExtendedPrice = Convert.ToDecimal(reader["ExtendedPrice"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
