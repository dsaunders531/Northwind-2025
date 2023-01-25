using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Identity.Web.Data;
using Northwind.Identity.Web.Models;
using Northwind.Identity.Web.Services;
using System.Configuration;

namespace Northwind.Identity.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
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
                options.ReturnUrlParameter = "r"; // this may help with man-in-the-middle-attacks and address spoofing               
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

            builder.Services.AddControllersWithViews();

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
            app.UseAuthentication();
            app.UseAuthorization();            
            /* Identity */

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}