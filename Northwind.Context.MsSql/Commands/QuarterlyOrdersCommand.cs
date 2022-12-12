// <copyright file="QuarterlyOrdersCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class QuarterlyOrdersCommand : SqlRunnerCommandWithoutUndo<IList<QuarterlyOrder>>
    {
        public QuarterlyOrdersCommand(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Quarterly Orders];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<QuarterlyOrder>> RunCommand(SqlCommand com)
        {
            List<QuarterlyOrder> result = new List<QuarterlyOrder>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new QuarterlyOrder()
                        {
                            CustomerId = reader["CustomerID"]?.ToString() ?? string.Empty,
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                            City = reader["City"]?.ToString() ?? string.Empty,
                            Country = reader["Country"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }

            return result;
        }
    }
}
