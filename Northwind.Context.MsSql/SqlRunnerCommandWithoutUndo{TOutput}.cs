// <copyright file="SqlRunnerCommandWithoutUndo{TOutput}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Patterns;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunnerCommandWithoutUndo<TOutput> : SqlRunnerCommand<TOutput>
        where TOutput : class
    {
        protected SqlRunnerCommandWithoutUndo(string connection)
            : base(connection)
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
