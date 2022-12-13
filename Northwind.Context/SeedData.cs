using Northwind.Context.Contexts;

namespace Northwind.Context
{
    public static class SeedData
    {
        /// <summary>
        /// Move the dates forward so the data becomes more useful.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void BringUpToDate(this NorthwindContext context, DateTime maxDate)
        {
            throw new NotImplementedException();
        }
    }
}
