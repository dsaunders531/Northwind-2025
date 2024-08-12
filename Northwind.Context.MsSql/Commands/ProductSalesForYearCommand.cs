// <copyright file="ProductSalesFor1997Command.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{

    internal class ProductSalesForYearCommand : SqlRunnerCommandWithoutUndo<IList<ProductSalesForYear>, int>
    {
        public ProductSalesForYearCommand(string connection, int parameters) : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "ProductSalesForYear";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            DateTime start = new DateTime(Parameters, 1, 1).Date;
            DateTime end = start.AddYears(1).Date.AddTicks(-1); // last moment of year

            com.Parameters.Add(new SqlParameter("@start", System.Data.SqlDbType.DateTime) { Value = start });
            com.Parameters.Add(new SqlParameter("@end", System.Data.SqlDbType.DateTime) { Value = end });
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
