using Microsoft.EntityFrameworkCore;
using Northwind.Context.Contexts;
using Northwind.Context.Models;
using Patterns.Extensions;

namespace Northwind.Context.InMemory.Contexts
{
#warning Do not use in production
    public sealed class NorthwindContextInMemory : NorthwindContext
    {
        public NorthwindContextInMemory(string pathToStateFiles) : base()
        {
            PathToStateFiles = pathToStateFiles;

            LoadState();
        }

        private string PathToStateFiles { get; set; }

        private string BaseFilePath()
        {
            return string.IsNullOrWhiteSpace(PathToStateFiles) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetType().ToString()) : PathToStateFiles;
        }

        public override void Dispose()
        {
            SaveState();

            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            SaveState();

            return base.DisposeAsync();
        }

        public override int SaveChanges()
        {
            SaveState();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SaveState();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveState();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SaveState();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("Northwind");
            }

            base.OnConfiguring(optionsBuilder);
        }

        private List<T> LoadTable<T>(string filename)
        {
            string filePath = Path.Combine(BaseFilePath(), filename);

            if (!Directory.Exists(BaseFilePath()))
            {
                Directory.CreateDirectory(BaseFilePath());
            }

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath).JsonConvert<List<T>>();
            }
            else
            {
                return new List<T>();
            }
        }

        private void LoadState()
        {
            Categories.AddRange(LoadTable<Category>("categories.json"));
            Customers.AddRange(LoadTable<Customer>("customers.json"));
            CustomerDemographics.AddRange(LoadTable<CustomerDemographic>("customerdemographics.json"));
            Employees.AddRange(LoadTable<Employee>("employees.json"));
            Orders.AddRange(LoadTable<Order>("orders.json"));
            OrderDetails.AddRange(LoadTable<OrderDetail>("orderDetails.json"));
            Products.AddRange(LoadTable<Product>("products.json"));
            Regions.AddRange(LoadTable<Region>("regions.json"));
            Shippers.AddRange(LoadTable<Shipper>("shippers.json"));
            Suppliers.AddRange(LoadTable<Supplier>("suppliers.json"));
            Territories.AddRange(LoadTable<Territory>("territories.json"));
        }

        private void SaveState()
        {
            File.WriteAllText(Path.Combine(BaseFilePath(), "categories.json"), Categories.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "customers.json"), Customers.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "customerdemographics.json"), CustomerDemographics.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "employees.json"), Employees.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "orders.json"), Orders.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "orderDetails.json"), OrderDetails.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "products.json"), Products.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "regions.json"), Regions.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "shippers.json"), Shippers.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "suppliers.json"), Suppliers.ToList().ToJson());
            File.WriteAllText(Path.Combine(BaseFilePath(), "territories.json"), Territories.ToList().ToJson());
        }
    }
}
