using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using Northwind.Context.Contexts;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Services;

namespace Northwind.Api
{

    /// <summary>
    /// Use this for testing.
    /// </summary>
    /// <remarks>You will also need to share the web app with the test library.
    /// Unload the project. Edit the project file by adding: <code><ItemGroup><InternalsVisibleTo Include = "Northwind.Api.Tests" /></ ItemGroup></code>
    /// Reload the project and it should work fine with the test.
    /// </remarks>
    public class NorthwindApiWebApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // shared extra set up goes here
            return base.CreateHost(builder);
        }
    }
}