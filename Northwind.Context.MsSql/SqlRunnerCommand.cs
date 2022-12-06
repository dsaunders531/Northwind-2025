using Microsoft.Data.SqlClient;
using Patterns;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunnerCommand<TOutput> : SqlRunner, IAsyncCommand<TOutput>
        where TOutput : class
    {
        public SqlRunnerCommand(string connection) : base(connection) { }
                
        public async Task<TOutput> Run()
        {
            TOutput result = Activator.CreateInstance<TOutput>();

            using (SqlConnection con = this.GetConnection())
            {
                await con.OpenAsync();

                try
                {
                    using (SqlCommand com = con.CreateCommand())
                    {
                        // add parameters and define command
                        this.DefineCommand(com);
                        this.DefineParameters(com);

                        // run the command and process the output.
                        result = await RunCommand(com);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await con.CloseAsync();
                    await con.DisposeAsync();
                }                                
            }

            return result;
        }

        public async Task Undo()
        {
            using (SqlConnection con = this.GetConnection())
            {
                try
                {
                    await con.OpenAsync();

                    using (SqlCommand com = con.CreateCommand())
                    {
                        // add parameters and define command
                        this.DefineUndoCommand(com);
                        this.DefineUndoParameters(com);

                        // run the command and process the output.
                        await RunUndoCommand(com);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await con.CloseAsync();
                    await con.DisposeAsync();
                }
            }
        }

        protected abstract void DefineCommand(SqlCommand com);

        protected abstract void DefineUndoCommand(SqlCommand com);

        protected abstract void DefineParameters(SqlCommand com);

        protected abstract void DefineUndoParameters(SqlCommand com);

        protected abstract Task<TOutput> RunCommand(SqlCommand com);

        protected abstract Task RunUndoCommand(SqlCommand com);
    }

    internal abstract class SqlRunnerCommandWithoutUndo<TOutput> : SqlRunnerCommand<TOutput>
        where TOutput : class
    {
        protected SqlRunnerCommandWithoutUndo(string connection) : base(connection)
        {
        }

        protected override void DefineUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }

        protected override void DefineUndoParameters(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }

        protected override Task RunUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }
    }

    internal abstract class SqlRunnerCommand<TOutput, TInput> : SqlRunnerCommand<TOutput>
        where TOutput : class
    {
        public SqlRunnerCommand(string connection, TInput parameters) : base(connection) 
        {
            this.Parameters = parameters;
        }

        protected TInput Parameters { get; set; }        
    }

    internal abstract class SqlRunnerCommandWithoutUndo<TOutput, TInput> : SqlRunnerCommand<TOutput, TInput>
        where TOutput : class        
    {
        protected SqlRunnerCommandWithoutUndo(string connection, TInput parameters) : base(connection, parameters)
        {
        }

        protected override void DefineUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }

        protected override void DefineUndoParameters(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }

        protected override Task RunUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{this.GetType().ToString} does not support undo operations.");
        }
    }
}
