// <copyright file="SqlRunnerCommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Patterns;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunnerCommand<TOutput> : SqlRunner, IAsyncCommand<TOutput>
        where TOutput : class
    {
        public SqlRunnerCommand(string connection)
            : base(connection)
        {
        }

        public async Task<TOutput> Run()
        {
            TOutput result = Activator.CreateInstance<TOutput>();

            using (SqlConnection con = GetConnection())
            {
                await con.OpenAsync();

                try
                {
                    using (SqlCommand com = con.CreateCommand())
                    {
                        // add parameters and define command
                        DefineCommand(com);
                        DefineParameters(com);

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
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    await con.OpenAsync();

                    using (SqlCommand com = con.CreateCommand())
                    {
                        // add parameters and define command
                        DefineUndoCommand(com);
                        DefineUndoParameters(com);

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
}
