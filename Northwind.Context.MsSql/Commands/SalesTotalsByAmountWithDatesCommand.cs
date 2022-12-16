// <copyright file="SalesTotalsByAmountCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using Patterns;

namespace Northwind.Context.MsSql.Commands
{

    internal class SalesTotalsByAmountWithDatesCommand : SqlRunnerCommandWithoutUndo<IList<SalesTotalsByAmount>, StartAndEndDate>
    {
        public SalesTotalsByAmountWithDatesCommand(string connection, StartAndEndDate parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "SalesByTotalsByAmount";
        }

        protected override void DefineParameters(SqlCommand com)
        {            
            com.Parameters.Add(new SqlParameter("@start", System.Data.SqlDbType.DateTime) { Value = Parameters.StartDate });
            com.Parameters.Add(new SqlParameter("@end", System.Data.SqlDbType.DateTime) { Value = Parameters.EndDate });
        }

        protected override async Task<IList<SalesTotalsByAmount>> RunCommand(SqlCommand com)
        {
            List<SalesTotalsByAmount> result = new List<SalesTotalsByAmount>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SalesTotalsByAmount()
                        {
                            SaleAmount = Convert.ToDecimal(reader["SaleAmount"]),
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
