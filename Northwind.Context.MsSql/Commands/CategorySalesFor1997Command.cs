// <copyright file="CategorySalesFor1997Command.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{
    internal class CategorySalesFor1997Command : SqlRunnerCommandWithoutUndo<IList<CategorySalesForYear>>
    {
        public CategorySalesFor1997Command(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Category Sales for 1997]";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
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
