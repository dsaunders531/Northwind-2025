using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;
using Patterns.Extensions;

namespace Northwind.Reporting.Rcl.Data
{
    /// <summary>
    /// File based report repository. Not for production use. Use a database and implement your own.
    /// </summary>
    internal class ReportRecordRepository : IReportRecordRepository
    {
        public ReportRecordRepository()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development" )
            {
                throw new ApplicationException("ReportRecordRepository is not suitable for production environments!");
            }

            Records = new Lazy<HashSet<ReportRecord>>(GetRecords, false);
        }

        private Lazy<HashSet<ReportRecord>> Records { get; set; }

        private string FilePath => Path.Combine(Environment.CurrentDirectory, "report_records.json");

        private static object SaveLock = new object();

        private HashSet<ReportRecord> GetRecords()
        {
            if (File.Exists(FilePath))
            {
                ReportRecord[] data = File.ReadAllText(FilePath).JsonConvert<ReportRecord[]>();

                return data.ToHashSet<ReportRecord>(new ReportRecordComparer());
            }
            else
            {
                return new HashSet<ReportRecord>();
            }
            
        }

        private void SaveRecords()
        {
            // only one save operation at a time
            if (Monitor.TryEnter(ReportRecordRepository.SaveLock, TimeSpan.FromSeconds(30)))
            {
                File.WriteAllText(FilePath, Records.Value.ToArray().ToJson());

                // this will get the records again from the updated file when next requested.
                Records = new Lazy<HashSet<ReportRecord>>(GetRecords, false);

                Monitor.Exit(ReportRecordRepository.SaveLock);               
            }
            else
            {
                throw new TimeoutException("Queue for saving report records is too long!");
            }
        }

        public Task<ReportRecord> Create(ReportRecord record)
        {
            if (record.Id == default || record.Id == 0)
            {
                if (Records.Value.Any())
                {
                    record.Id = Records.Value.Max(m => m.Id) + 1;
                }
                else
                {
                    // first record
                    record.Id = 1;
                }                
            }

            Records.Value.Add(record);

            SaveRecords();

            return Task.FromResult(record);
        }

        public Task<bool> Delete(long id)
        {
            Records.Value.RemoveWhere(w => w.Id == id);

            SaveRecords();

            return Task.FromResult(true);
        }

        public Task<ReportRecord> Fetch(long id)
        {
            return Task.FromResult(Records.Value.Where(w => w.Id == id).FirstOrDefault() ?? throw new KeyNotFoundException($"Could not find {id}"));
        }

        public Task<ReportRecord[]> Fetch(Func<ReportRecord, bool> predicate)
        {
            return Task.FromResult(Records.Value.Where(predicate).ToArray() ?? Array.Empty<ReportRecord>());
        }

        public Task<bool> Update(ReportRecord record)
        {
            Delete(record.Id);

            Create(record);

            return Task.FromResult(true);
        }
    }
}
