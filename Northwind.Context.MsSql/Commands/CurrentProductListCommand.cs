// <copyright file="CurrentProductListCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class CurrentProductListCommand : SqlRunnerCommandWithoutUndo<IList<CurrentProductList>>
    {
        public CurrentProductListCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Current Product List];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<CurrentProductList>> RunCommand(SqlCommand com)
        {
            List<CurrentProductList> result = new List<CurrentProductList>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CurrentProductList()
                        {
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                        });
                    }
                }
            }

            return result;
        }
    }
}
