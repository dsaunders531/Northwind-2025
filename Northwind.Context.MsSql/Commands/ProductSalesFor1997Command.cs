using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class ProductSalesFor1997Command : SqlRunnerCommandWithoutUndo<IList<ProductSalesFor1997>>
    {
        public ProductSalesFor1997Command(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Product Sales for 1997];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<ProductSalesFor1997>> RunCommand(SqlCommand com)
        {
            List<ProductSalesFor1997> result = new List<ProductSalesFor1997>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ProductSalesFor1997()
                        {
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            ProductSales = Convert.ToDecimal(reader["ProductSales"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
