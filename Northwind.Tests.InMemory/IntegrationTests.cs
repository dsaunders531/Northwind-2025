using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Services;

namespace Northwind.Tests.InMemory
{
    public class IntegrationTests : Sql.IntegrationTests
    {
        [SetUp]
        public override void Setup()
        {
            NorthwindContext = new NorthwindContextInMemory(string.Empty);
            NorthwindService = new NorthwindService(NorthwindContext);
        }
    }
}
