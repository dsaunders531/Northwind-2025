using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Northwind.Context.Contexts;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Services;

namespace Northwind.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            logger.Debug("Startup");

            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                // Add logging (NLog for this app)
                // When using cloud hosting - use thier own logger and wire up some email alerts for exceptions.
                // eg: when using AWS - log using thier own. The messages appear in cloudwatch.
                // Then use a lambda to look for errors and send a message.
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();
                builder.Host.UseNLog();

                // Add services to the container.
                Program.ConfigureBusinessServices(builder);

                builder.Services.AddControllers();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                WebApplication app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

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
            
            // TODO
            // ADD https only middleware.
            // Add authentication middleware.            
            // see https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/
        }

        private static void ConfigureBusinessServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<NorthwindContext>(new NorthwindContextInMemory(string.Empty));
            builder.Services.AddTransient<INorthwindService, NorthwindService>();
            builder.Services.AddTransient<INorthwindProductsService, NorthwindProductsService>();
        }
    }
}