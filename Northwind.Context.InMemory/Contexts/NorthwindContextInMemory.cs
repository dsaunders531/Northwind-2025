using Microsoft.EntityFrameworkCore;
using Northwind.Context.Contexts;
using Northwind.Context.Models;
using Patterns.Extensions;

namespace Northwind.Context.InMemory.Contexts
{
#warning Do not use in production
    public class NorthwindContextInMemory : NorthwindContext
    {
        public NorthwindContextInMemory(string pathToStateFiles) : base()
        {
            this.PathToStateFiles = pathToStateFiles;

            this.LoadState();
        }
        
        private string PathToStateFiles { get; set; }
        
        private string BaseFilePath()
        {            
            return string.IsNullOrWhiteSpace(this.PathToStateFiles) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.GetType().ToString()) : this.PathToStateFiles;
        }

        public override void Dispose()
        {
            this.SaveState();

            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            this.SaveState();

            return base.DisposeAsync();
        }

        public override int SaveChanges()
        {
            this.SaveState();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.SaveState();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.SaveState();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.SaveState();

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
            string filePath = Path.Combine(this.BaseFilePath(), filename);

            if (!Directory.Exists(this.BaseFilePath()))
            {
                Directory.CreateDirectory(this.BaseFilePath());
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
            this.Categories.AddRange(this.LoadTable<Category>("categories.json"));            
            this.Customers.AddRange(this.LoadTable<Customer>("customers.json"));
            this.CustomerDemographics.AddRange(this.LoadTable<CustomerDemographic>("customerdemographics.json"));
            this.Employees.AddRange(this.LoadTable<Employee>("employees.json"));
            this.Orders.AddRange(this.LoadTable<Order>("orders.json"));
            this.OrderDetails.AddRange(this.LoadTable<OrderDetail>("orderDetails.json"));
            this.Products.AddRange(this.LoadTable<Product>("products.json"));
            this.Regions.AddRange(this.LoadTable<Region>("regions.json"));
            this.Shippers.AddRange(this.LoadTable<Shipper>("shippers.json"));
            this.Suppliers.AddRange(this.LoadTable<Supplier>("suppliers.json"));
            this.Territories.AddRange(this.LoadTable<Territory>("territories.json"));
        }

        private void SaveState()
        {
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "categories.json"), this.Categories.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "customers.json"), this.Customers.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "customerdemographics.json"), this.CustomerDemographics.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "employees.json"), this.Employees.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "orders.json"), this.Orders.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "orderDetails.json"), this.OrderDetails.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "products.json"), this.Products.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "regions.json"), this.Regions.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "shippers.json"), this.Shippers.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "suppliers.json"), this.Suppliers.ToList().ToJson());
            File.WriteAllText(Path.Combine(this.BaseFilePath(), "territories.json"), this.Territories.ToList().ToJson());
        }
    }
}
