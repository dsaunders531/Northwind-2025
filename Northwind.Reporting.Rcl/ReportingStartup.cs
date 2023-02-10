using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Rcl.Data;
using Northwind.Reporting.Rcl.Factories;
using Northwind.Reporting.Services;

namespace Northwind.Reporting.Rcl
{
    public static class ReportingStartup
    {
        private static ReportRunnerService? ReportRunner { get; set; }

        public static void AddReporting(this WebApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<IReportFactory, ReportFactory>();

            builder.Services.TryAddSingleton<IReportRecordRepository, ReportRecordRepository>();
        }

        public static void UseReporting(this WebApplication app)
        {
            // In the real world - the report runner should be run as a background operation
            // a windows service or unix daemon is best. An app run from a cron job or windows task manager
            // would be ok at a push.
            if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty) == "Development")
            {
                ReportingStartup.ReportRunner = new ReportRunnerService(app.Services.GetRequiredService<IReportRecordRepository>(), 
                                                                        app.Services.GetRequiredService<ILogger<ReportRunnerService>>(), 
                                                                        app.Services.GetRequiredService<IReportFactory>());
            }
        }
    }
}
