using Northwind.Context.Models.Database;

namespace Northwind.Context.Extensions
{

    public static class EmployeeExtensions
    {
        public static string FullName(this Employee employee)
        {
            return string.Concat(employee.FirstName ?? string.Empty, " ", employee.LastName ?? string.Empty);
        }
    }
}
