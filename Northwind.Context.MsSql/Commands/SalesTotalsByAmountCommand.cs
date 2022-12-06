using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesTotalsByAmountCommand : SqlRunnerCommandWithoutUndo<IList<SalesTotalsByAmount>>
    {
        public SalesTotalsByAmountCommand(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Sales Totals by Amount];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
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
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
