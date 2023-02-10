namespace Northwind.Reporting.Enums
{
    /// <summary>
    /// Reports will have a frequency
    /// </summary>
    public enum ReportFrequency
    {
        Immediate = 0,
        Daily = 1, // Daily needs a time of day and to create a new report record after each run. Report parameters for start and end will need adjusting.
        Weekly = 2, // Weekly needs a day of week parameter and create a new report record after each run. Report parameters for start and end will need adjusting.
        Monthly = 3 // Monthly needs a day of month (up to 28) and create a new report record after each run. Report parameters for start and end will need ajusting.               
    }
}
