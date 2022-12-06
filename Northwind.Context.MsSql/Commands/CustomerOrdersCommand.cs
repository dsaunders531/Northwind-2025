// <copyright file="CustomerOrdersCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class CustomerOrdersCommand : SqlRunnerCommandWithoutUndo<IList<CustomerOrders>, string>
    {
        public CustomerOrdersCommand(string connection, string parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "CustOrdersOrders";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@CustomerId", System.Data.SqlDbType.NChar, 5) { Value = Parameters });
        }

        protected override async Task<IList<CustomerOrders>> RunCommand(SqlCommand com)
        {
            List<CustomerOrders> result = new List<CustomerOrders>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CustomerOrders()
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(reader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
