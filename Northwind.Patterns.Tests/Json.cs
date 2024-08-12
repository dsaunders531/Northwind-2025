using Patterns.Extensions;

namespace Northwind.Patterns.Tests
{
    public partial class Json
    {
        [Test]
        public void JsonExtensionTests()
        {
            TestThing thing = new TestThing()
            {
                WholeNumber = int.MaxValue,
                Date = DateTime.MaxValue,
                DecimalNumber = decimal.MinValue,
                String = "Hello World!",
                Related = null,
                Boolean = false
            };

            string value = thing.ToJson();

            Assert.That(value, Has.Length.GreaterThan(0));

            TestThing deserialized = value.JsonConvert<TestThing>();

            Assert.IsNotNull(deserialized);

            Assert.That(deserialized.WholeNumber, Is.EqualTo(thing.WholeNumber));
            Assert.That(deserialized.Date, Is.EqualTo(thing.Date));
            Assert.That(deserialized.DecimalNumber, Is.EqualTo(thing.DecimalNumber));
            Assert.That(deserialized.String, Is.EqualTo(thing.String));
            Assert.That(deserialized.Related, Is.EqualTo(thing.Related));
            Assert.That(deserialized.Boolean, Is.EqualTo(thing.Boolean));
        }
    }
}