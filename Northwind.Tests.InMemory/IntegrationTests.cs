using Northwind.Context;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Services;

namespace Northwind.Tests.InMemory
{
    public class IntegrationTests : Sql.IntegrationTests
    {
        [SetUp]
        public override void Setup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            NorthwindContext = new NorthwindContextInMemory(string.Empty);
            NorthwindService = new NorthwindService(NorthwindContext);

            NorthwindContext.Database.EnsureCreated();
            //NorthwindContext.Database.Migrate(); // not supported for in-memory
            NorthwindContext.BringUpToDate(DateTime.UtcNow);
        }
    }
}
