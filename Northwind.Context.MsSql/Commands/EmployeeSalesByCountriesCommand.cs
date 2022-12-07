// <copyright file="EmployeeSalesByCountriesCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using Patterns;

namespace Northwind.Context.MsSql.Commands
{
    internal class EmployeeSalesByCountriesCommand : SqlRunnerCommandWithoutUndo<IList<EmployeeSalesByCountry>, StartAndEndDate>
    {
        public EmployeeSalesByCountriesCommand(string connection, StartAndEndDate parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "[Employee Sales by Country]";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@Beginning_Date", System.Data.SqlDbType.DateTime) { Value = Parameters.StartDate });
            com.Parameters.Add(new SqlParameter("@Ending_Date", System.Data.SqlDbType.DateTime) { Value = Parameters.EndDate });
        }

        protected override async Task<IList<EmployeeSalesByCountry>> RunCommand(SqlCommand com)
        {
            List<EmployeeSalesByCountry> result = new List<EmployeeSalesByCountry>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new EmployeeSalesByCountry()
                        {
                            Country = reader["Country"]?.ToString() ?? string.Empty,
                            LastName = reader["LastName"].ToString() ?? string.Empty,
                            FirstName = reader["FirstName"].ToString() ?? string.Empty,
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            SaleAmount = Convert.ToDecimal(reader["SaleAmount"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
