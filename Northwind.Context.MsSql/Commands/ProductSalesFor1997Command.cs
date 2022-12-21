// <copyright file="ProductSalesFor1997Command.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{
    internal class ProductSalesFor1997Command : SqlRunnerCommandWithoutUndo<IList<ProductSalesForYear>>
    {
        public ProductSalesFor1997Command(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Product Sales for 1997];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<ProductSalesForYear>> RunCommand(SqlCommand com)
        {
            List<ProductSalesForYear> result = new List<ProductSalesForYear>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ProductSalesForYear()
                        {
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            ProductSales = Convert.ToDecimal(reader["ProductSales"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
