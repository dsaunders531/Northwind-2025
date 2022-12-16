// <copyright file="QuarterlyOrdersWithDatesCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using Northwind.Context.MsSql.Parameters;

namespace Northwind.Context.MsSql.Commands
{
    internal class QuarterlyOrdersWithDatesCommand : SqlRunnerCommandWithoutUndo<IList<QuarterlyOrder>, YearAndQuarterParameters>
    {
        public QuarterlyOrdersWithDatesCommand(string connection, YearAndQuarterParameters parameters) : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "QuarterlyOrders";
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

        protected override async Task<IList<QuarterlyOrder>> RunCommand(SqlCommand com)
        {
            List<QuarterlyOrder> result = new List<QuarterlyOrder>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new QuarterlyOrder()
                        {
                            CustomerId = reader["CustomerID"]?.ToString() ?? string.Empty,
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                            City = reader["City"]?.ToString() ?? string.Empty,
                            Country = reader["Country"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }

            return result;
        }
    }
}
