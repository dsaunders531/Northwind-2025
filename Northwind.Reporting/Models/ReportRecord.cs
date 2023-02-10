using Northwind.Reporting.Enums;
using Northwind.Reporting.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Northwind.Reporting.Models
{
    /// <summary>
    /// The state of each report will need to be persisted somewhere.
    /// They will not be run in the MVC process as this will be too slow.
    /// </summary>
    public record ReportRecord : IReportOptions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The user who created the record.
        /// </summary>
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string ReportName { get; set; } = string.Empty;

        /// <summary>
        /// Reports will have parameters each report may have different properties - these need to be stored.
        /// </summary>
        /// <remarks>All report parameters need to inherit from ReportParameterBase otherwise there will be problems re-scheduling a report.</remarks>
        public string ReportParametersJson { get; set; } = string.Empty;

        /// <summary>
        /// The location of the report (on the server) where the report is located.
        /// </summary>
        public string? OutputPath { get; set; }

        /// <summary>
        /// The time the report will need to be run
        /// </summary>
        [Required]
        public DateTime RunTime { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Created;

        [Required]
        public ReportFrequency Frequency { get; set; } = ReportFrequency.Immediate;

        public ReportWriter OutputFormat { get; set; } = ReportWriter.Csv;

        /// <summary>
        /// The frequency value when the report will be run.
        /// </summary>
        /// <remarks>
        ///     For weekly frequency - needs to be the day of the week (Sunday = 0)
        ///     For monthly frequency - needs to be the day of the month (1 to 28).
        ///     Monthly frequency does not apply to days above 29 as months are unequal lengths.
        ///     Most of the time, monthly reports are run early in the month.
        /// </remarks>
        [Range(minimum: 0, maximum: 55)]
        public int? FrequencyWeeklyMonthly { get; set; }
    }

    public class ReportRecordComparer : IEqualityComparer<ReportRecord>
    {
        public bool Equals(ReportRecord? x, ReportRecord? y)
        {
            return (x?.Id ?? 0) == (y?.Id ?? 0);
        }

        public int GetHashCode([DisallowNull] ReportRecord obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
