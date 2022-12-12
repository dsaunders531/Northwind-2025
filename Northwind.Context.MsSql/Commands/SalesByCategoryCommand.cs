// <copyright file="SalesByCategoryCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByCategoryCommand : SqlRunnerCommandWithoutUndo<IList<SalesByCategory>>
    {
        public SalesByCategoryCommand(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Sales by Category];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<SalesByCategory>> RunCommand(SqlCommand com)
        {
            List<SalesByCategory> result = new List<SalesByCategory>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SalesByCategory()
                        {
                            CategoryId = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            ProductSales = Convert.IsDBNull(reader["ProductSales"]) ? default(decimal?) : Convert.ToDecimal(reader["ProductSales"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
