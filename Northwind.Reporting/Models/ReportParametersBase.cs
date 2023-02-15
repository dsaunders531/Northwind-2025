using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Spreadsheet;
using Northwind.Reporting.Enums;
using Northwind.Reporting.Interfaces;

namespace Northwind.Reporting.Models
{

    /// <summary>
    /// All report parameter classes will need a start and end date otherwise it will not be possible to create a recurring report.
    /// </summary>
    public abstract class ReportParametersBase : IReportParametersBase
    {
        /// <summary>
        /// A start date and end date is needed for one-off reports
        /// </summary>        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// A start date and end date is needed for one-off reports
        /// </summary>        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// A recurring report will need to calculate the start and end date based on the value selected here.
        /// </summary>
        [EnumDataType(typeof(ReportPeriod))]
        public ReportPeriod? ReportPeriod { get; set; }
    }

    /// <summary>
    /// This is intended as a helper for testing - your reports need to inherit from ReportParametersBase
    /// </summary>
    public class ReportParameters : ReportParametersBase { }
}
