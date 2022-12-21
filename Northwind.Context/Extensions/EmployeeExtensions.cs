using Northwind.Context.Models.Database;

namespace Northwind.Context.Extensions
{

    internal static class EmployeeExtensions
    {
        public static string FullName(this Employee employee)
        {
            return string.Concat(employee.FirstName ?? string.Empty, " ", employee.LastName ?? string.Empty);
        }
    }
}
