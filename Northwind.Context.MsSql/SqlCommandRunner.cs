using Microsoft.Data.SqlClient;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlCommandRunner<TOutput> : SqlRunner
        where TOutput : class
    {
        public SqlCommandRunner(string connection) : base(connection) { }
                
        public async Task<TOutput> Run()
        {
            TOutput result = Activator.CreateInstance<TOutput>();

            using (SqlConnection con = this.GetConnection())
            {
                using (SqlCommand com = con.CreateCommand())
                {
                    // add parameters and define command
                    this.DefineCommand(com);
                    this.DefineParameters(com);

                    await con.OpenAsync();

                    // run the command and process the output.
                    result = await RunCommand(com);
                }

                await con.CloseAsync();
                await con.DisposeAsync();
            }

            return result;
        }

        protected abstract void DefineCommand(SqlCommand com);

        protected abstract void DefineParameters(SqlCommand com);

        protected abstract Task<TOutput> RunCommand(SqlCommand com);
    }

    internal abstract class SqlCommandRunner<TOutput, TInput> : SqlCommandRunner<TOutput>
        where TOutput : class
        where TInput : class
    {
        public SqlCommandRunner(string connection, TInput parameters) : base(connection) 
        {
            this.Parameters = parameters;
        }

        protected TInput Parameters { get; set; }        
    }
}
