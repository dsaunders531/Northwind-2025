// <copyright file="SummaryOfSalesByYearCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SummaryOfSalesByYearCommand : SqlRunnerCommandWithoutUndo<IList<SummaryOfSalesByYear>>
    {
        public SummaryOfSalesByYearCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Summary of Sales by Year];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
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
