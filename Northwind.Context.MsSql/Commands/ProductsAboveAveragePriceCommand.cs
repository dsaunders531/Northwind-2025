// <copyright file="ProductsAboveAveragePriceCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models.Reporting;

namespace Northwind.Context.MsSql.Commands
{
    internal class ProductsAboveAveragePriceCommand : SqlRunnerCommandWithoutUndo<IList<ProductsAboveAveragePrice>>
    {
        public ProductsAboveAveragePriceCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Products Above Average Price];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<ProductsAboveAveragePrice>> RunCommand(SqlCommand com)
        {
            List<ProductsAboveAveragePrice> result = new List<ProductsAboveAveragePrice>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ProductsAboveAveragePrice()
                        {
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
