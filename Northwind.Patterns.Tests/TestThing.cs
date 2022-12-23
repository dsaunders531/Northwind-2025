namespace Northwind.Patterns.Tests
{
    public partial class Json
    {
        internal class TestThing
        {            
            public int? WholeNumber { get; set; }

            public DateTime Date { get; set; }
            
            public  decimal? DecimalNumber { get; set; }
            
            public string? String { get; set; }
            
            public bool Boolean { get; set; }

            public TestThing? Related { get; set; }
        }
    }
}