// <copyright file="NorthwindContextSql.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Northwind.Context.Contexts;

namespace Northwind.Context.MsSql.Contexts
{
    public sealed class NorthwindContextSql : NorthwindContext
    {
        public NorthwindContextSql()
        {
        }

        public NorthwindContextSql(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP10\\SQLEXPRESS;Database=Northwind-2025-Local;Trusted_Connection=true;MultipleActiveResultSets=true;");
            }
        }
    }
}
