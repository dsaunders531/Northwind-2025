namespace Northwind.Reporting.Tests
{
    internal class MockServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }
    }
}