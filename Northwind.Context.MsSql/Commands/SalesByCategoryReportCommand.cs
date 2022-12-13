// <copyright file="SalesByCategoryReportCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByCategoryReportCommand : SqlRunnerCommandWithoutUndo<IList<SaleByCategoryReport>, SalesByCateogryReportCommandParameters>
    {
        public SalesByCategoryReportCommand(string connection, SalesByCateogryReportCommandParameters parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "SalesByCategory";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 15) { Value = Parameters.CategoryName });
            com.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.NVarChar, 4) { Value = Parameters.Year.ToString() });
        }

        protected override async Task<IList<SaleByCategoryReport>> RunCommand(SqlCommand com)
        {
            List<SaleByCategoryReport> result = new List<SaleByCategoryReport>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SaleByCategoryReport()
                        {
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            TotalPurchased = Convert.ToDecimal(reader["TotalPurchase"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
