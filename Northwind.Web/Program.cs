using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Northwind.Api.Client;
using Northwind.Context.Interfaces;
using Northwind.Reporting.Rcl;
using Northwind.Security.ActionFilters;
using System.IdentityModel.Tokens.Jwt;

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

                builder.Services.AddAntiforgery();

                builder.Services.AddControllersWithViews(options => {                    
                    options.Filters.Add<HttpsOnlyActionFilter>(); // reject anything on http
                    options.Filters.Add<ContentSecurityActionFilter>(); // add csp
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>(); // expect a validation token on all endpoints apart from HEAD, GET, OPTIONS, TRACE                   
                });
               
                if (!builder.Environment.IsDevelopment())
                {
                    builder.Services.AddHsts(options => {
                        options.Preload = true;
                        options.IncludeSubDomains = true;
                        options.MaxAge = TimeSpan.FromDays(360);                        
                    });
                }
               
                IMvcBuilder razorPagesBuilder = builder.Services.AddRazorPages();

                if (builder.Environment.IsDevelopment())
                {                    
                    razorPagesBuilder.AddRazorRuntimeCompilation();
                }

                /* Add identity hosted on Northwind.Idenity.Web */
                JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
                
                builder.Services
                    .AddAuthentication(options =>
                    {
                        options.DefaultScheme = "Cookies";
                        options.DefaultChallengeScheme = "oidc";
                    })
                    .AddCookie("Cookies")
                    .AddOpenIdConnect("oidc", options =>
                    {
                        // Where identity is served from Northwind.Identity.Web
                        options.Authority = "https://localhost:7153";

                        options.ClientId = "northwind-web-user";
                        options.ClientSecret = "secret";
                        options.ResponseType = "code";

                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        
                        options.GetClaimsFromUserInfoEndpoint = true;

                        options.SaveTokens = true;
                    });

                // Add reporting
                builder.AddReporting();

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

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseReporting();

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