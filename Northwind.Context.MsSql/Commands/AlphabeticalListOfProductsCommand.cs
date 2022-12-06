using Microsoft.Data.SqlClient;
using Northwind.Context.Models;
using System.Net;

namespace Northwind.Context.MsSql.Commands
{
    internal class AlphabeticalListOfProductsCommand : SqlRunnerCommandWithoutUndo<IList<AlphabeticalListOfProduct>>
    {
        public AlphabeticalListOfProductsCommand(string connection) : base(connection) { }

        protected override void DefineCommand(SqlCommand com)
        {
            com.CommandType = System.Data.CommandType.Text;
            com.CommandText = "select * from [Alphabetical list of products];";
        }

        protected override void DefineParameters(SqlCommand com)
        {
            // no parameters
        }
       
        protected override async Task<IList<AlphabeticalListOfProduct>> RunCommand(SqlCommand com)
        {
            List<AlphabeticalListOfProduct> result = new List<AlphabeticalListOfProduct>();

            using (SqlDataReader reader = await com.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new AlphabeticalListOfProduct() { 
                            CategoryId = Convert.ToInt32(reader["CategoryID"]),
                            Discontinued = Convert.ToBoolean(reader["Discontinued"]),
                            CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            ProductName = reader["ProductName"]?.ToString() ?? string.Empty,
                            QuantityPerUnit = reader["QuantityPerUnit"].ToString(),
                            ReorderLevel = Convert.ToInt16(reader["ReorderLevel"]),
                            SupplierId = Convert.ToInt32(reader["SupplierID"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            UnitsInStock = Convert.ToInt16(reader["UnitsInStock"]),
                            UnitsOnOrder = Convert.ToInt16(reader["UnitsOnOrder"])                            
                        });
                    }
                }
            }

            return result;
        }        
    }
}
