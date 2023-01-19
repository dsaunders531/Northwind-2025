using Northwind.Context.Models.Database;
using Northwind.Context.Models.Interfaces;

namespace Northwind.Context.Models.Api
{

    public class ProductApi : IProduct
    {
        public ProductApi()
        {
            ProductName = string.Empty;
        }

        private ProductApi(Product model)
        {
            CategoryId = model.CategoryId;
            Discontinued = model.Discontinued;
            ProductId = model.ProductId;
            ProductName = model.ProductName ?? string.Empty;
            QuantityPerUnit = model.QuantityPerUnit ?? "0";
            UnitPrice = model.UnitPrice ?? decimal.MaxValue;
            UnitsInStock = model.UnitsInStock ?? 0;
        }

        public static ProductApi Create(Product model)
        {
            return new ProductApi(model);
        }

        public int? CategoryId { get; set; }
        public bool Discontinued { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
    }
}
