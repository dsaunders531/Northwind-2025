// <copyright file="SalesByCategoryCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByCategoryCommand : SqlRunnerCommandWithoutUndo<IList<SaleByCategoryReport>>
    {
        public SalesByCategoryCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "SalesByCategory";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no paramters
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
