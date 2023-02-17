using Microsoft.Extensions.Logging;
using Northwind.Reporting.Enums;
using Northwind.Reporting.Extensions;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;
using System.Timers;

namespace Northwind.Reporting.Services
{

    /// <summary>
    /// Reports need to be run by a service which needs to be always running.
    /// </summary>
    /// <remarks>This needs to be a windows service, unix daemon or a container which starts and runs often.</remarks>
    public class ReportRunnerService
    {
        public ReportRunnerService(IReportRecordRepository repository, ILogger<ReportRunnerService> logger, IReportFactory reportFactory)
        {
            Logger = logger;
            Repository = repository;
            ReportFactory = reportFactory;

            PendingReportTimer = new System.Timers.Timer()
            {
                Interval = TimeSpan.FromMinutes(3).TotalMilliseconds,
                AutoReset = true,
                Enabled = true
            };

            PendingReportTimer.Elapsed += Timer_Elapsed;

            Jobs = new Queue<ReportRecord>();
                    
            ProcessQueueTimer = new System.Timers.Timer()
            {
                Interval = TimeSpan.FromSeconds(30).TotalMilliseconds,
                AutoReset = false,
                Enabled = false
            };

            ProcessQueueTimer.Elapsed += ProcessQueueTimer_Elapsed;

            // add any pending or processing items to the queue (these will not have completed or have a way of completing)
            ReportRecord[] pending = Repository.Fetch(f => f.Status == ReportStatus.Pending || f.Status == ReportStatus.Running)
                                .GetAwaiter().GetResult();

            if (pending?.Any() ?? false)
            {
                foreach (ReportRecord item in pending)
                {
                    Jobs.Enqueue(item);
                }

                ProcessQueueTimer.Start();
            }            
        }

        private IReportFactory ReportFactory { get; set; }

        private IReportRecordRepository Repository { get; set; }

        private System.Timers.Timer PendingReportTimer { get; set; }

        private System.Timers.Timer ProcessQueueTimer { get; set; }

        private ILogger<ReportRunnerService> Logger { get; set; }

        private Queue<ReportRecord> Jobs { get; set; }

        private void ProcessQueueTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (Jobs.Any())
            {
                try
                {
                    // switch the timer off
                    ProcessQueueTimer.Stop();

                    ReportRecord? reportRecord = default;

                    // run all the reports in the queue one at a time.
                    // while this is slower overall, it makes sure that resources are not over-used.                    
                    while (Jobs.TryDequeue(out reportRecord))
                    {
                        try
                        {
                            reportRecord.Status = ReportStatus.Running;
                            _ = Repository.Update(reportRecord).GetAwaiter().GetResult();

                            // Run the report
                            reportRecord.OutputPath = ReportFactory.GetReport(reportRecord.ReportName).Run(reportRecord).GetAwaiter().GetResult().AbsolutePath;

                            reportRecord.Status = ReportStatus.Completed;

                            _ = Repository.Update(reportRecord).GetAwaiter().GetResult();

                            // see if the report needs to be resheduled.
                            if (reportRecord.Frequency != ReportFrequency.Immediate)
                            {
                                // create the next instance.
                                _ = Repository.Create(reportRecord.Clone());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, $"Could not run report {reportRecord.ReportName}. {ex.GetType()}: {ex.Message}");

                            reportRecord.Status = ReportStatus.Error;

                            _ = Repository.Update(reportRecord).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error running reports");
                }                
            }
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            PendingReportTimer.Stop();

            try
            {
                // Are there any reports to run?
                ReportRecord[] reports = Repository.Fetch(w => w.RunTime <= DateTime.UtcNow
                                                            && w.Status == ReportStatus.Created)
                                                        .GetAwaiter().GetResult();

                if (reports.Any())
                {
                    foreach (ReportRecord item in reports.OrderByDescending(o => o.RunTime))
                    {
                        try
                        {
                            item.Status = ReportStatus.Pending;
                            Jobs.Enqueue(item);
                            _ = Repository.Update(item).GetAwaiter().GetResult();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "Error scheduling reports");
                        }
                    }

                    if (!ProcessQueueTimer.Enabled)
                    {
                        ProcessQueueTimer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error running reports");

            }
            finally
            {
                PendingReportTimer.Start();
            }
        }
    }
}
