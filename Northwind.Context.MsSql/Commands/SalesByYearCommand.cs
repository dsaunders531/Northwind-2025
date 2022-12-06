using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using Patterns;

namespace Northwind.Context.MsSql.Commands
{
    internal class SalesByYearCommand : SqlRunnerCommandWithoutUndo<IList<SaleByYear>, StartAndEndDate>
    {
        public SalesByYearCommand(string connection, StartAndEndDate parameters) : base(connection, parameters)
        {
        }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "[Sales by Year]";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            com.Parameters.Add(new SqlParameter("@Beginning_Date", System.Data.SqlDbType.DateTime) { Value = this.Parameters.StartDate });
            com.Parameters.Add(new SqlParameter("@Ending_Date", System.Data.SqlDbType.DateTime) { Value = this.Parameters.EndDate });
        }

        protected override async Task<IList<SaleByYear>> RunCommand(SqlCommand com)
        {
            List<SaleByYear> result = new List<SaleByYear>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new SaleByYear() { 
                            ShippedDate = Convert.ToDateTime(reader["ShippedDate"]),
                            OrderId = Convert.ToInt32(reader["OrderID"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            Year = Convert.ToInt32(reader["Year"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
