using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Northwind.Context.Contexts;
using Northwind.Context.InMemory.Contexts;
using Northwind.Context.Interfaces;
using Northwind.Context.Services;
using Northwind.Security.ActionFilters;
using Patterns.Extensions;

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
                // Get the configuration.
                IConfiguration configuration = new ConfigurationBuilder()
                                                .AddEnvironmentVariables()
                                                .AddJsonFile("appsettings.json", false, false)
                                                .Build();

                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                // Add logging (NLog for this app)
                // When using cloud hosting - use thier own logger and wire up some email alerts for exceptions.
                // eg: when using AWS - log using thier own. The messages appear in cloudwatch.
                // Then use a lambda to look for errors and send a message.
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();
                builder.Host.UseNLog();

                // use cors
                string[] corsAllowOurSites = configuration.GetSection("CORS")["AllowedOrigins"].ToString().Split(",");

                builder.Services.AddCors(options => {
                    options.AddPolicy(name: "ForOurWebSite",
                        policy => {
                            policy.WithOrigins(corsAllowOurSites)
                                    .AllowAnyMethod()
                                    .WithMethods("GET","POST","PUT","DELETE","OPTIONS","HEAD","PATCH")
                                    //.AllowAnyHeader()
                                    .WithHeaders("Content-Type")
                                    //.AllowCredentials()
                                    .SetPreflightMaxAge(TimeSpan.FromSeconds(240));
                        });
                });

                // Add services to the container.
                Program.ConfigureBusinessServices(builder);

                builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<HttpsOnlyActionFilter>();
                    options.Filters.Add(new AuthorizeFilter()); // Authorise everything by default
                });
                
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                if (!builder.Environment.IsDevelopment())
                {
                    builder.Services.AddHsts(options => {
                        options.Preload = true;
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(360);
                    });
                };
                
                /* Add Identity Server Authorisation (see Northwind.Identity.Web) */
                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", opts =>
                    {
                        opts.Authority = "https://localhost:7153"; // Northwind.Identity.Web
                        opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidateAudience = false
                        };
                    });

                /* End Identity Server Auth*/

                WebApplication app = builder.Build();

                app.UseSwagger();
                app.UseSwaggerUI();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();

                app.UseCors("ForOurWebSite"); // this must go above auth and map controllers

                app.UseAuthentication();
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
        }

        private static void ConfigureBusinessServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<NorthwindContext>(new NorthwindContextInMemory(string.Empty));
            builder.Services.AddTransient<INorthwindService, NorthwindService>();
            builder.Services.AddTransient<INorthwindProductsService, NorthwindProductsService>();
        }
    }
}