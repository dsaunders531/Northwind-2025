using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Northwind.Identity.Web.Models;

namespace Northwind.Identity.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IPersistedGrantDbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
        
        public DbSet<Key> Keys { get; set; }

        // this is needed by IPersistedGrantDbContext   
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP10\\SQLEXPRESS;Initial Catalog=Northwind-Identity;Integrated Security=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");

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