using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Northwind.Identity.Web.Data;
using Northwind.Identity.Web.Models;
using Northwind.Identity.Web.Services;
using Northwind.Security.ActionFilters;

namespace Northwind.Identity.Web
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

                // Add logging (NLog for this app)
                // When using cloud hosting - use thier own logger and wire up some email alerts for exceptions.
                // eg: when using AWS - log using thier own. The messages appear in cloudwatch.
                // Then use a lambda to look for errors and send a message.
                builder.Logging.ClearProviders();
                builder.Logging.AddNLog();
                builder.Host.UseNLog();

                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
                }

                builder.Services.AddAntiforgery();

                builder.Services.AddControllersWithViews(options => {
                    options.Filters.Add<HttpsOnlyActionFilter>(); // reject anything on http
                    options.Filters.Add<ContentSecurityActionFilter>(); // add csp
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>(); // expect a validation token on all endpoints apart from HEAD, GET, OPTIONS, TRACE                   
                });

                // Add caching
                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddDistributedMemoryCache();
                }
                else
                {
                    // it looks like the table has to be created manually:
                    // dotnet sql-cache create "{conn str}" cache DistributedCache
                    // there is an overview of the schema here:
                    // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0
                    string memCacheConnString = builder.Configuration.GetConnectionString("MemCache") ?? throw new InvalidOperationException("Connection string 'MemCache' not found.");

                    builder.Services.AddDistributedSqlServerCache(opts =>
                    {
                        opts.ConnectionString = memCacheConnString;
                        opts.SchemaName = "cache";
                        opts.TableName = "DistributedCache";
                    });
                }

                /* Identity */
                // Add services to the container.
                var connectionString = builder.Configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'Identity' not found.");

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

                builder.Services
                    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                        options.Lockout.AllowedForNewUsers = false;
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireDigit = true;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequiredUniqueChars = 2;
                        options.SignIn.RequireConfirmedPhoneNumber = false;
                        options.SignIn.RequireConfirmedEmail = false;
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddDefaultUI()
                    .AddDefaultTokenProviders()
                    .AddPersonalDataProtection<LookupProtector, LookupProtectorKeyRing>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();                

                builder.Services.Configure<PasswordHasherOptions>(opts =>
                {
                    opts.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                    opts.IterationCount = 10000;
                });

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.Cookie.Name = "Northwind";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Identity/Account/Login";
                    //options.ReturnUrlParameter = "r"; // this may help with man-in-the-middle-attacks and address spoofing               
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    options.SlidingExpiration = true;
                });

                // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-7.0
                // By default, the Data Protection system isolates apps from one another based on their content root paths, even if they share the same physical key repository. This isolation prevents the apps from understanding each other's protected payloads.

                // To share protected payloads among apps:
                //    Configure SetApplicationName in each app with the same value.
                //    Use the same version of the Data Protection API stack across the apps. Perform either of the following in the apps' project files:
                //    Reference the same shared framework version via the Microsoft.AspNetCore.App metapackage.
                //    Reference the same Data Protection package version.
                string dataProtectionConnStr = builder.Configuration.GetConnectionString("DataProtection") ?? throw new InvalidOperationException("Connection string 'DataProtection' not found.");

                builder.Services.AddDbContext<DataProtectionDbContext>(options =>
                    options.UseSqlServer(dataProtectionConnStr));

                builder.Services
                    .AddDataProtection()
                    .PersistKeysToDbContext<DataProtectionDbContext>()
                    .SetApplicationName("Northwind");                        
                /* Identity Ends */

                /* Add Identity Server */
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes                

                builder.Services.AddIdentityServer()
                    .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                    .AddInMemoryClients(IdentityServerConfig.Clients)
                    .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                    .AddAspNetIdentity<ApplicationUser>();
                /* End Identity Server */                                

                IMvcBuilder mvcBuilder = builder.Services.AddControllersWithViews();

                if (builder.Environment.IsDevelopment())
                {
                    mvcBuilder.AddRazorRuntimeCompilation();
                }

                var app = builder.Build();
                
                // Create databasees and perform migrations
                // in load-balanced instances - only the 'primary' instance should do this.
                if (Convert.ToBoolean(Environment.GetEnvironmentVariable("PERFORM_MIGRATIONS") ?? string.Empty))
                {
                    using (ApplicationDbContext context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                                                                                        .UseSqlServer(builder.Configuration.GetConnectionString("Identity"))
                                                                                        .Options))
                    {
                        context.Database.Migrate();
                    }

                    using (DataProtectionDbContext context = new DataProtectionDbContext(new DbContextOptionsBuilder<DataProtectionDbContext>()
                                                                                                .UseSqlServer(builder.Configuration.GetConnectionString("DataProtection"))
                                                                                                .Options))
                    {
                        context.Database.Migrate();
                    }
                }
                // End migrations

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint(); // this should not be needed if the above has been run.
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                /* Identity */
                Program.AddIdentitySeedData(app);
                                
                app.UseAuthentication();
                /* Identity Server */
                app.UseIdentityServer();
                app.UseAuthorization();                                
                /* Identity */

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapRazorPages(); //.RequireAuthorization();

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

        /// <summary>
        /// Add the seed data for identity.
        /// </summary>
        /// <param name="app"></param>
        private static void AddIdentitySeedData(WebApplication app)
        {
            if (Convert.ToBoolean(Environment.GetEnvironmentVariable("PERFORM_MIGRATIONS") ?? string.Empty))
            {
                using (IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    RoleManager<ApplicationRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                     
                    string[] defaultRoles = new string[] { "User", "UserAdministrator" };

                    foreach (string role in defaultRoles)
                    {
                        if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                        {
                            IdentityResult createdRoleResult = roleManager.CreateAsync(new ApplicationRole() { Id = Guid.NewGuid(), Name = role, NormalizedName = role.Normalize() }).GetAwaiter().GetResult();

                            if (!createdRoleResult.Succeeded)
                            {
                                throw new ApplicationException("Could not create default role!");
                            }
                        }
                    }

                    // create a seed uesr - this should be an admin.
                    // Use this account to configure a real user to be the admin.
                    // Then prevent this account from logging in. (remove roles and add a lockout)                 
                    ApplicationUser defaultUser = userManager.FindByNameAsync("admin@northwind.com").GetAwaiter().GetResult();

                    if (defaultUser == default)
                    {
                        // Create the user.
                        defaultUser = new ApplicationUser() { Email = "admin@northwind.com", UserName = "admin@northwind.com", EmailConfirmed = true };
                        
                        IdentityResult result = userManager.CreateAsync(defaultUser, "Password1!").GetAwaiter().GetResult();
                        
                        if (result.Succeeded)
                        {
                            _ = userManager.AddToRoleAsync(defaultUser, "UserAdministrator").GetAwaiter().GetResult();
                        }
                    }
                }
            }                
        }
    }
}