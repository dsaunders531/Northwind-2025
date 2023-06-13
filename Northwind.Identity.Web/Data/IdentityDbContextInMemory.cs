using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Northwind.Identity.Web.Data
{
    public class IdentityDbContextInMemory : IdentityDbContext
    {
        public IdentityDbContextInMemory() : base() { }

        public IdentityDbContextInMemory(DbContextOptions<IdentityDbContextInMemory> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning InMemory database is not suitable for production use and has no data persistance between sessions.
                optionsBuilder.UseInMemoryDatabase("Identity");

                optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}