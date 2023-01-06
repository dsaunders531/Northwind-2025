using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Northwind.Api.Client;
using Northwind.Context.Interfaces;

namespace Northwind.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("Startup");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add logging(NLog for this app)
                // When using cloud hosting - use thier own logger and wire up some email alerts for exceptions.
                // eg: when using AWS - log using thier own. The messages appear in cloudwatch.
                // Then use a lambda to look for errors and send a message.
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();
                builder.Host.UseNLog();

                // Add services to the container.
                IConfiguration configuration = new ConfigurationBuilder()
                                                .AddEnvironmentVariables()
                                                .AddJsonFile("appsettings.json", false, false)
                                                .Build();

                builder.Services.AddHttpClient();

                builder.Services.AddSingleton<INorthwindProductsService, NorthwindApiProxy>();

                builder.Services.AddControllersWithViews();
                builder.Services.AddRazorPages();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapDefaultControllerRoute();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                app.MapRazorPages();

                app.MapFallbackToPage("/App"); // this acts as a fallback

                app.Run();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }            
        }
    }
}