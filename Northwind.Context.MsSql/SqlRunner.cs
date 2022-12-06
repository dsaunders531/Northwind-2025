// <copyright file="SqlRunner.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

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
