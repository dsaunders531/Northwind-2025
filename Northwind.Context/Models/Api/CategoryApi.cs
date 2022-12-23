using Northwind.Context.Models.Database;
using Northwind.Context.Models.Interfaces;

namespace Northwind.Context.Models.Api
{
    public class CategoryApi : ICategory
    {
        public CategoryApi() 
        {
            CategoryName = string.Empty;
        }

        private CategoryApi(Category model)
        {
            CategoryName = model.CategoryName;
            CategoryId = model.CategoryId;
            Description = model.Description ?? string.Empty;
        }

        public static CategoryApi Create(Category model)
        {
            return new CategoryApi(model);
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
