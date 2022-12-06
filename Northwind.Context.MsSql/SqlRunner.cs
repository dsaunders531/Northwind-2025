using Microsoft.Data.SqlClient;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunner
    {
        public SqlRunner(string connection)
        {
            Connection = connection;
        }

        protected string Connection { get; set; }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(Connection);
        }
    }
}
