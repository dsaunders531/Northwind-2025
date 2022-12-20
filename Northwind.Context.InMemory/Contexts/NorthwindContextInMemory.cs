using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.Context.Contexts;
using Northwind.Context.Models;
using Patterns.Extensions;
using File = System.IO.File;

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

        private bool Loaded = false;

        /// <summary>
        /// You can store the datafiles in the bin folder or in the users roaming profile
        /// </summary>
        /// <returns></returns>
        private string BaseFilePath()
        {
            return Path.Combine(Environment.CurrentDirectory,"Resources")
                                ?? (string.IsNullOrWhiteSpace(PathToStateFiles) 
                                        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetType().ToString()) 
                                        : PathToStateFiles);
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
            //SaveState();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SaveState();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //SaveState();

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

                optionsBuilder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
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
            if (!Loaded)
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

                base.SaveChanges();

                this.Loaded = true;
            }
        }

        private void SaveState(bool locked)
        {
            if (!locked)
            {
                throw new SynchronizationLockException("This process needs to be locked before running!");
            }

            // get the data and avoid writing out all the relations.
            File.WriteAllText(Path.Combine(BaseFilePath(), "categories.json"), Categories.Select(s => new Category()
            {
                CategoryId = s.CategoryId,
                CategoryName = s.CategoryName,
                Description = s.Description,
                Picture = s.Picture
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "customers.json"), Customers.Select(s => new Customer()
            {
                City = s.City,
                CompanyName = s.CompanyName,
                Address = s.Address,
                ContactName = s.ContactName,
                ContactTitle = s.ContactTitle,
                Country = s.Country,
                CustomerId = s.CustomerId,
                PostalCode = s.PostalCode,
                Fax = s.Fax,
                Phone = s.Phone,
                Region = s.Region
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "customerdemographics.json"), CustomerDemographics.Select(s => new CustomerDemographic()
            {
                CustomerDesc = s.CustomerDesc,
                CustomerTypeId = s.CustomerTypeId
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "employees.json"), Employees.Select(s => new Employee()
            {
                EmployeeId = s.EmployeeId,
                Address = s.Address,
                BirthDate = s.BirthDate,
                Region = s.Region,
                Extension = s.Extension,
                City = s.City,
                Country = s.Country,
                FirstName = s.FirstName,
                HireDate = s.HireDate,
                HomePhone = s.HomePhone,
                LastName = s.LastName,
                Notes = s.Notes,
                Photo = s.Photo,
                PhotoPath = s.PhotoPath,
                PostalCode = s.PostalCode,
                ReportsTo = s.ReportsTo,
                Title = s.Title,
                TitleOfCourtesy = s.TitleOfCourtesy
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "orders.json"), Orders.Select(s => new Order()
            {
                ShipAddress = s.ShipAddress,
                ShipCity = s.ShipCity,
                ShipCountry = s.ShipCountry,
                ShipName = s.ShipName,
                ShippedDate = s.ShippedDate,
                ShipPostalCode = s.ShipPostalCode,
                ShipRegion = s.ShipRegion,
                ShipVia = s.ShipVia,
                CustomerId = s.CustomerId,
                EmployeeId = s.EmployeeId,
                Freight = s.Freight,
                OrderDate = s.OrderDate,
                OrderId = s.OrderId,
                RequiredDate = s.RequiredDate
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "orderDetails.json"), OrderDetails.Select(s => new OrderDetail()
            {
                Discount = s.Discount,
                OrderId = s.OrderId,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "products.json"), Products.Select(s => new Product()
            {
                SupplierId = s.SupplierId,
                UnitsInStock = s.UnitsInStock,
                CategoryId = s.CategoryId,
                Discontinued = s.Discontinued,
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                QuantityPerUnit = s.QuantityPerUnit,
                ReorderLevel = s.ReorderLevel,
                UnitPrice = s.UnitPrice,
                UnitsOnOrder = s.UnitsOnOrder
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "regions.json"), Regions.Select(s => new Region()
            {
                RegionDescription = s.RegionDescription,
                RegionId = s.RegionId
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "shippers.json"), Shippers.Select(s => new Shipper()
            {
                ShipperId = s.ShipperId,
                CompanyName = s.CompanyName,
                Phone = s.Phone
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "suppliers.json"), Suppliers.Select(s => new Supplier()
            {
                Address = s.Address,
                SupplierId = s.SupplierId,
                City = s.City,
                CompanyName = s.CompanyName,
                ContactTitle = s.ContactTitle,
                ContactName = s.ContactName,
                Country = s.Country,
                Fax = s.Fax,
                HomePage = s.HomePage,
                Phone = s.Phone,
                PostalCode = s.PostalCode,
                Region = s.Region
            }).ToArray().ToJson());

            File.WriteAllText(Path.Combine(BaseFilePath(), "territories.json"), Territories.Select(s => new Territory()
            {
                RegionId = s.RegionId,
                TerritoryDescription = s.TerritoryDescription,
                TerritoryId = s.TerritoryId
            }).ToArray().ToJson());
        }

        private void SaveState()
        {
            this.SaveState(true);
        }
    }
}
