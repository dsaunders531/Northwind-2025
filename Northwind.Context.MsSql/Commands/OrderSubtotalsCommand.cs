using Microsoft.Data.SqlClient;
using Northwind.Context.Models;

namespace Northwind.Context.MsSql.Commands
{
    internal class OrderSubtotalsCommand : SqlRunnerCommandWithoutUndo<IList<OrderSubtotal>>
    {
        public OrderSubtotalsCommand(string connection) : base(connection)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [dbo].[Order Subtotals];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }

        protected override async Task<IList<OrderSubtotal>> RunCommand(SqlCommand com)
        {
            List<OrderSubtotal> result = new List<OrderSubtotal>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new OrderSubtotal()
                        {
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
