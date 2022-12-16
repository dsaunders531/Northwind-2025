// <copyright file="SalesByCategoryReportDatesCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using Northwind.Context.MsSql.Parameters;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByCategoryReportDatesCommand : SqlRunnerCommandWithoutUndo<IList<SalesByCategory>, YearAndQuarterParameters>
    {
        public SalesByCategoryReportDatesCommand(string connection, YearAndQuarterParameters parameters) : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "SalesByCategoryReport";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            DateTime start = new DateTime(Parameters.Year, 1, 1).Date;

            switch (Parameters.Quarter)
            {
                case 2:
                    start = start.AddMonths(4);
                    break;
                case 3:
                    start = start.AddMonths(7);
                    break;
                case 4:
                    start = start.AddMonths(10);
                    break;
                default:
                    // do nothing
                    break;
            }

            DateTime end = start.AddMonths(4).Date.AddTicks(-1);

            com.Parameters.Add(new SqlParameter("@start", System.Data.SqlDbType.DateTime) { Value = start });
            com.Parameters.Add(new SqlParameter("@end", System.Data.SqlDbType.DateTime) { Value = end });
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
