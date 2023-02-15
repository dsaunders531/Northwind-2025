using Northwind.Reporting.Factories;
using Patterns.Extensions;

namespace Northwind.Reporting.Interfaces
{
    public abstract class Report
    {
        public Report(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected IServiceProvider ServiceProvider { get; private set; }
        
        /// <summary>
        /// The name for the report.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// A brief description of the report.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The route to the config page
        /// </summary>
        public abstract string ConfigPageRoute { get; }

        public abstract Task<Uri> Run(IReportOptions parameters);
    }


    /// <summary>
    /// All reports will be different but there must be a common way to run them.
    /// They need to be listable in the UI so they need a description
    /// </summary>
    public abstract class Report<TOutput, TParameters> : Report
        where TOutput : class
        where TParameters : class
    {
        public Report(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public string ReportOutputBase => Environment.GetEnvironmentVariable("REPORTWRITER_BASE_DIR") ?? @"c:\temp";

        /// <summary>
        /// Run the report
        /// </summary>
        /// <param name="reportParameters"></param>
        /// <returns>The location of the report.</returns>
        public async override Task<Uri> Run(IReportOptions reportParameters)
        {
            try
            {
                TParameters parameters = reportParameters.ReportParametersJson.JsonConvert<TParameters>();

                return await ReportWriterFactory.GetWriter<TOutput>(reportParameters.OutputFormat).Write(await Run(parameters));
            }
            catch (Exception e)
            {
                // write out any exceptions to a file
                string errorFilePath = Path.Combine(ReportOutputBase, $"{Guid.NewGuid()}.txt");

                File.WriteAllText(errorFilePath, $"Error running report!\r\n{e.GetType()}\r\n{e.Message}\r\n{e.ToJson()}");

                return new Uri(errorFilePath);
            }
        }

        /// <summary>
        /// Run the report with parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <remarks>This is where the work of running the report is done.</remarks>
        public abstract Task<IEnumerable<TOutput>> Run(TParameters parameters);
    }
}
