using Northwind.Reporting.Enums;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Reporting.Rcl.Data
{
    /// <summary>
    /// Each report has different parameters.
    /// However, there are some common things like run frequency which needs to be the same 
    /// for all the config pages.
    /// </summary>
    public class ReportConfig<TParameters> : IReportConfig
        where TParameters : class
    {
        public ReportConfig()
        {
            this.Parameters = Activator.CreateInstance<TParameters>();
        }

        [Required]
        public ReportFrequency Frequency { get; set; } = ReportFrequency.Immediate;

        /// <summary>
        /// When the frequency is weekly or monthly, the user should select a value
        /// from either ReportFrequencyWeekly or ReportFrequencyMonthly.     
        /// </summary>
        public int? FrequencyWeeklyMonthly { get; set; }

        [Required]
        public ReportWriter ReportWriter { get; set; } = ReportWriter.Csv;

        [Required]
        public TParameters Parameters { get; set; }
    }
}
