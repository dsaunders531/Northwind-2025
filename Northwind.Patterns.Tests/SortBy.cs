using Patterns;
using Patterns.Extensions;

namespace Northwind.Patterns.Tests
{
    public class SortByTests
    {
        /// <summary>
        /// Test the sortby enumerator. It uses binary options.
        /// </summary>
        [Test]
        public void SortByTest()
        {
            // To prevent lots of different variations of sort, use a binary value.
            SortBy sort = SortBy.Ascending | SortBy.Name;

            Assert.That((int)sort, Is.EqualTo(5));

            Assert.That(sort.IsAscending(), Is.True);

            Assert.That(sort.Simplified(), Is.EqualTo(SortBy.Name));
        }
    }
}