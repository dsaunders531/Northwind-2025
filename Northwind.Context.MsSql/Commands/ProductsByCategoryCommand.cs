// <copyright file="ProductsByCategoryCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class ProductsByCategoryCommand : SqlRunnerCommandWithoutUndo<IList<ProductsByCategory>>
    {
        public ProductsByCategoryCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Products by Category];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<ProductsByCategory>> RunCommand(SqlCommand com)
        {
            List<ProductsByCategory> result = new List<ProductsByCategory>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ProductsByCategory()
                        {
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            QuantityPerUnit = reader["QuantityPerUnit"]?.ToString() ?? string.Empty,
                            UnitsInStock = Convert.ToInt16(reader["UnitsInStock"]),
                            Discontinued = Convert.ToBoolean(reader["Discontinued"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
