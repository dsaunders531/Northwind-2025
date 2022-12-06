namespace Northwind.Context.Models
{
    public class SaleByYear
    {
        public DateTime ShippedDate { get; set; }
        public int OrderId { get; set; }
        public decimal Subtotal { get; set; }
        public int Year { get; set; }
    }
}
