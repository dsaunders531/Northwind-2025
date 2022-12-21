using Northwind.Context.Models.Database;
using Northwind.Context.Models.Interfaces;

namespace Northwind.Context.Models.Api
{

    public class ProductApi : IProduct
    {
        public ProductApi()
        {
            this.ProductName = string.Empty;
        }

        private ProductApi(Product model)
        {
            this.CategoryId = model.CategoryId;
            this.Discontinued = model.Discontinued;
            this.ProductId = model.ProductId;
            this.ProductName = model.ProductName ?? string.Empty;
            this.QuantityPerUnit = model.QuantityPerUnit ?? "0";
            this.UnitPrice = model.UnitPrice ?? decimal.MaxValue;
            this.UnitsInStock = model.UnitsInStock ?? 0;
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
