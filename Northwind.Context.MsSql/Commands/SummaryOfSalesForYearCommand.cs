// <copyright file="SummaryOfSalesByYearCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{

    internal class SummaryOfSalesForYearCommand : SqlRunnerCommandWithoutUndo<IList<SummaryOfSalesByYear>, int>
    {
        public SummaryOfSalesForYearCommand(string connection, int parameter)
            : base(connection, parameter)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "SummarySalesTotalByYear";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            DateTime start = new DateTime(Parameters, 1, 1).Date;
            DateTime end = start.AddYears(1).Date.AddTicks(-1);

            com.Parameters.Add(new SqlParameter("@start", System.Data.SqlDbType.DateTime) { Value = start });
            com.Parameters.Add(new SqlParameter("@end", System.Data.SqlDbType.DateTime) { Value = end });
        }

        protected override async Task<IList<SummaryOfSalesByYear>> RunCommand(SqlCommand com)
        {
            List<SummaryOfSalesByYear> result = new List<SummaryOfSalesByYear>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SummaryOfSalesByYear()
                        {
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
