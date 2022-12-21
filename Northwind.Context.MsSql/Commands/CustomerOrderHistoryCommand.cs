// <copyright file="CustomerOrderHistoryCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{
    internal class CustomerOrderHistoryCommand : SqlRunnerCommandWithoutUndo<IList<CustomerOrderHistory>, string>
    {
        public CustomerOrderHistoryCommand(string connection, string parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "CustOrderHist";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@CustomerId", System.Data.SqlDbType.NChar, 5) { Value = Parameters });
        }

        protected override async Task<IList<CustomerOrderHistory>> RunCommand(SqlCommand com)
        {
            List<CustomerOrderHistory> result = new List<CustomerOrderHistory>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CustomerOrderHistory()
                        {
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            Total = Convert.ToInt32(reader["Total"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
