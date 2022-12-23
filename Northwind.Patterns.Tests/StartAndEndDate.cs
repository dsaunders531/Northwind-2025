using Patterns;

namespace Northwind.Patterns.Tests
{
    public class StartAndEndDateTests
    {
        [Test]
        public void StartAndEndDateYearTests()
        {
            // test years
            StartAndEndDate value = new StartAndEndDate(2022);

            Assert.That(value.StartDate, Is.EqualTo(new DateTime(2022, 1, 1).Date));
            Assert.That(value.EndDate, Is.EqualTo(new DateTime(2023, 1, 1).Date.AddTicks(-1)));
        }

        [Test]
        public void StartAndEndDateYearAndQuarterTests()
        {
            // test year and quarter
            StartAndEndDate value = new StartAndEndDate(2022, 3);

            Assert.That(value.StartDate, Is.EqualTo(new DateTime(2022, 7, 1).Date));
            Assert.That(value.EndDate, Is.EqualTo(new DateTime(2022, 10, 1).Date.AddTicks(-1)));
        }
    }
}