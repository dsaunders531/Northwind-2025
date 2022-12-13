// <copyright file="SqlRunnerCommandWithoutUndo{TOutput,TInput}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunnerCommandWithoutUndo<TOutput, TInput> : SqlRunnerCommand<TOutput, TInput>
        where TOutput : class
    {
        protected SqlRunnerCommandWithoutUndo(string connection, TInput parameters)
            : base(connection, parameters)
        {
        }

        protected override void DefineUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{GetType().ToString} does not support undo operations.");
        }

        protected override void DefineUndoParameters(SqlCommand com)
        {
            throw new NotSupportedException($"{GetType().ToString} does not support undo operations.");
        }

        protected override Task RunUndoCommand(SqlCommand com)
        {
            throw new NotSupportedException($"{GetType().ToString} does not support undo operations.");
        }
    }
}
