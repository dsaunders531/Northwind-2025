// <copyright file="TenMostExpensiveProductsCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class TenMostExpensiveProductsCommand : SqlRunnerCommandWithoutUndo<IList<MostExpensiveProduct>>
    {
        public TenMostExpensiveProductsCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "[Ten Most Expensive Products]";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<MostExpensiveProduct>> RunCommand(SqlCommand com)
        {
            List<MostExpensiveProduct> result = new List<MostExpensiveProduct>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new MostExpensiveProduct()
                        {
                            Name = reader["TenMostExpensiveProducts"]?.ToString() ?? string.Empty,
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        });
                    }
                }
            }

            return result;
        }
    }
}
