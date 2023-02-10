using System.ComponentModel.DataAnnotations;

namespace Northwind.Reporting.Models
{
    /// <summary>
    /// All report parameter classes will need a start and end date otherwise it will not be possible to create a recurring report.
    /// </summary>
    public class ReportParametersBase
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
