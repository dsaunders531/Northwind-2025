using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Identity.Web.Data
{
    public class DataProtectionDbContextInMemory : DataProtectionDbContext
    {
        public DataProtectionDbContextInMemory() : base() { }

        public DataProtectionDbContextInMemory(DbContextOptions<DataProtectionDbContextInMemory> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning InMemory database is not suitable for production use and has no data persistance between sessions.
                optionsBuilder.UseInMemoryDatabase("DataProtection");

                optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
