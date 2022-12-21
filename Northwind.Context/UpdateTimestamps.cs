using Northwind.Context.Contexts;
using Northwind.Context.Models.Database;

namespace Northwind.Context
{
    public static class UpdateTimestamps
    {
        /// <summary>
        /// Move the dates forward so the data becomes more useful.
        /// </summary>
        /// <param name="context"></param>
        public static void BringUpToDate(this NorthwindContext context, DateTime targetDate)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                throw new NotSupportedException("Updating the database timestamps can only be done in Development mode!");
            }
            else
            {
                // Only the orders table needs to be changed
                DateTime maxDate = context.Orders.Max(m => m.OrderDate) ?? DateTime.UtcNow;

                TimeSpan difference = targetDate - maxDate;

                if (difference.Days >= 20)
                {
                    // bring the data up to date
                    foreach (Order item in context.Orders)
                    {
                        item.OrderDate = item.OrderDate.HasValue ? item.OrderDate.Value.AddDays(difference.Days) : item.OrderDate;
                        item.RequiredDate = item.RequiredDate.HasValue ? item.RequiredDate.Value.AddDays(difference.Days) : item.RequiredDate;
                        item.ShippedDate = item.ShippedDate.HasValue ? item.ShippedDate.Value.AddDays(difference.Days) : item.ShippedDate;

                        context.Update(item);
                    }

                    context.SaveChanges();
                }
            }
            
        }
    }
}
