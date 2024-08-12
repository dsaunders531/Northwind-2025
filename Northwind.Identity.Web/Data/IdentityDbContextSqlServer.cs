using Microsoft.EntityFrameworkCore;

namespace Northwind.Identity.Web.Data
{
    public class IdentityDbContextSqlServer : IdentityDbContext
    {
        public IdentityDbContextSqlServer() : base() { }

        public IdentityDbContextSqlServer(DbContextOptions<IdentityDbContextSqlServer> options) : base(options) { }

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
        }
    }
}