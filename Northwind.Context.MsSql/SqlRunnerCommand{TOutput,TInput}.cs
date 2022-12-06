// <copyright file="SqlRunnerCommand{TOutput,TInput}.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.Data.SqlClient;
using Patterns;

namespace Northwind.Context.MsSql
{
    internal abstract class SqlRunnerCommand<TOutput, TInput> : SqlRunnerCommand<TOutput>
        where TOutput : class
    {
        public SqlRunnerCommand(string connection, TInput parameters)
            : base(connection)
        {
            Parameters = parameters;
        }

        protected TInput Parameters { get; set; }
    }
}
