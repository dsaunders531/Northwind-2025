using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class CategorySalesFor1997Command : SqlRunnerCommandWithoutUndo<IList<CategorySalesFor1997>>
    {
        public CategorySalesFor1997Command(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Category Sales for 1997]";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<CategorySalesFor1997>> RunCommand(SqlCommand com)
        {
            List<CategorySalesFor1997> result = new List<CategorySalesFor1997>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CategorySalesFor1997()
                        {
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            CategorySales = Convert.IsDBNull(reader["CategorySales"]) ? new decimal?() : Convert.ToDecimal(reader["CategorySales"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
