using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class SummaryOfSalesByQuarterCommand : SqlRunnerCommandWithoutUndo<IList<SummaryOfSalesByQuarter>>
    {
        public SummaryOfSalesByQuarterCommand(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Summary of Sales by Quarter];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<SummaryOfSalesByQuarter>> RunCommand(SqlCommand com)
        {
            List<SummaryOfSalesByQuarter> result = new List<SummaryOfSalesByQuarter>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SummaryOfSalesByQuarter() { 
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
