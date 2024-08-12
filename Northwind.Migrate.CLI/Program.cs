using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Northwind.Context.Contexts;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.MsSql.Contexts;
using System.Diagnostics;

namespace Northwind.Migrate.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Migrate database - press any key to continue.");
            Console.ReadKey(true);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // migrate from SQL to InMemory
                // You could use this tool to migrate any datasource to another.
                Program.PerformMigration();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error! {ex.GetType()}: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();

                Console.WriteLine($"Migration Complete: took {stopwatch.Elapsed.TotalMinutes.ToString("F2")} minutes.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(true);
            }
        }

        private static void PerformMigration()
        {
            ConfigurationManager config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json", false);
            string connection = config.GetConnectionString("sourceDatabase");

            DbContextOptions<NorthwindContext> contextOptions = new DbContextOptionsBuilder<NorthwindContext>().UseSqlServer(connection).Options;

            string pathToOutputFiles = string.Empty;

            using (NorthwindContext sourceContext = new NorthwindContextSql(contextOptions))
            {
                using (NorthwindContext targetContext = new NorthwindContextInMemory(pathToOutputFiles))
                {
                    // targetContext.Database.Migrate(); // create tables etc. if supported

                    // check there is no data in the target!
                    bool carryOn = !(targetContext.Categories.Any()
                                    && targetContext.Customers.Any()
                                    && targetContext.Employees.Any()
                                    && targetContext.Orders.Any()
                                    && targetContext.OrderDetails.Any()
                                    && targetContext.Products.Any()
                                    && targetContext.Regions.Any()
                                    && targetContext.Shippers.Any()
                                    && targetContext.Suppliers.Any()
                                    && targetContext.Territories.Any());

                    if (!carryOn)
                    {
                        throw new NotSupportedException("There is data in the target already!");
                    }

                    using (IDbContextTransaction tran = targetContext.Database.BeginTransaction())
                    {
                        try
                        {
                            // copy all the data from the soure to the target.

                            // If you have stored procs, views or functions - copy these using migrations.

                            // All we are doing here is copying data.

                            // When doing this with different contexts, the table order may be important...

                            targetContext.Categories.AddRange(sourceContext.Categories);
                            targetContext.Customers.AddRange(sourceContext.Customers);
                            targetContext.CustomerDemographics.AddRange(sourceContext.CustomerDemographics);
                            targetContext.Employees.AddRange(sourceContext.Employees);
                            targetContext.Orders.AddRange(sourceContext.Orders);
                            targetContext.OrderDetails.AddRange(sourceContext.OrderDetails);
                            targetContext.Products.AddRange(sourceContext.Products);
                            targetContext.Regions.AddRange(sourceContext.Regions);
                            targetContext.Shippers.AddRange(sourceContext.Shippers);
                            targetContext.Suppliers.AddRange(sourceContext.Suppliers);
                            targetContext.Territories.AddRange(sourceContext.Territories);

                            tran.Commit();
                            targetContext.SaveChanges();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}