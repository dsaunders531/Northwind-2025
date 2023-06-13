using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Northwind.Identity.Web.Models;

namespace Northwind.Identity.Web.Data
{
    /// <summary>
    /// Base class for Identity context.
    /// Use this as a base for different providers.
    /// </summary>
    public abstract class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IPersistedGrantDbContext
    {
        public IdentityDbContext() : base() { }

        public IdentityDbContext(DbContextOptions options) : base(options) { }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public DbSet<Key> Keys { get; set; }

        // this is needed by IPersistedGrantDbContext   
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Key>()
                .HasNoKey()
                .HasIndex(i => new { i.Id, i.Created });

            builder.Entity<DeviceFlowCodes>()
                .HasNoKey()
                .HasIndex(i => new { i.SessionId, i.ClientId, i.SubjectId });

            builder.Entity<PersistedGrant>()
                .HasNoKey()
                .HasIndex(i => new { i.Key, i.SubjectId, i.ClientId, i.SessionId });
        }
    }
}