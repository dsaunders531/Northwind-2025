// <copyright file="ProductSalesFor1997Command.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{

    internal class CategorySalesForYearCommand : SqlRunnerCommandWithoutUndo<IList<CategorySalesForYear>, int>
    {
        public CategorySalesForYearCommand(string connection, int parameters) : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "CategorySalesForYear";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            DateTime start = new DateTime(Parameters, 1, 1).Date;
            DateTime end = start.AddYears(1).Date.AddTicks(-1); // last moment of year

            com.Parameters.Add(new SqlParameter("@start", System.Data.SqlDbType.DateTime) { Value = start });
            com.Parameters.Add(new SqlParameter("@end", System.Data.SqlDbType.DateTime) { Value = end });
        }

        protected override async Task<IList<CategorySalesForYear>> RunCommand(SqlCommand com)
        {
            List<CategorySalesForYear> result = new List<CategorySalesForYear>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CategorySalesForYear()
                        {
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            CategorySales = Convert.IsDBNull(reader["CategorySales"]) ? default(decimal?) : Convert.ToDecimal(reader["CategorySales"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
