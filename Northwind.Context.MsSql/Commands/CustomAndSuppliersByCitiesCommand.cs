// <copyright file="CustomAndSuppliersByCitiesCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class CustomAndSuppliersByCitiesCommand : SqlRunnerCommandWithoutUndo<IList<CustomerAndSuppliersByCity>>
    {
        public CustomAndSuppliersByCitiesCommand(string connection)
            : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Customer and Suppliers by City];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<CustomerAndSuppliersByCity>> RunCommand(SqlCommand com)
        {
            List<CustomerAndSuppliersByCity> result = new List<CustomerAndSuppliersByCity>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CustomerAndSuppliersByCity()
                        {
                            City = reader["City"].ToString(),
                            CompanyName = reader["CompanyName"]?.ToString() ?? string.Empty,
                            ContactName = reader["ContactName"].ToString(),
                            Relationship = reader["Relationship"]?.ToString() ?? string.Empty,
                        });
                    }
                }
            }

            return result;
        }
    }
}
