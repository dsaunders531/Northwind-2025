namespace Northwind.Reporting.Enums
{
    /// <summary>
    /// Reports will have a status
    /// </summary>
    public enum ReportStatus
    {
        Created = 0,
        Pending = 1,
        Running = 2,
        Completed = 3,
        Error = 4
    }
}
