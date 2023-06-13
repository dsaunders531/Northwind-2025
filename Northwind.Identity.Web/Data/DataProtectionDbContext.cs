using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Identity.Web.Data
{
    /// <summary>
    /// Base class for dataprotection context. 
    /// Use this as a base for different database providers.
    /// </summary>
    public abstract class DataProtectionDbContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionDbContext() : base() { }
        
        public DataProtectionDbContext(DbContextOptions options) : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }        
    }
}
