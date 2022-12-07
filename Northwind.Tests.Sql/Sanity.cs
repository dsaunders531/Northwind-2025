namespace Northwind.Tests.Sql
{
    public class Sanity
    {
        [Test]
        public void SanityTest()
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}