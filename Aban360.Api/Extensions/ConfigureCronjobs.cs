using Aban360.Api.Cronjobs;
using Hangfire;

namespace Aban360.Api.Extensions
{
    internal static class ConfigureCronjobs
    {
        const int _fillMeterLifeHour = 4;
        const int _removeOldReportHour = 2; const int _removeOldReportMin=45;
        internal static void AddCronjobs(this IConfiguration configuration)
        {
            RemoveOldReports(configuration);
            FillMeterLife();
        }
        private static void RemoveOldReports(IConfiguration configuration)
        {
            int tresholdDay = int.Parse(configuration["FileManagement:ExcelExpireDay"]);
            string excelFilePath = configuration["FileManagement:ExcelPath"].ToString();
            string searchPattern = "*.xlsx";//todo: get from config         

            RecurringJob.AddOrUpdate(
                "RemoveOldReports",
                () => FileRemover.DeleteOldFiles(tresholdDay, excelFilePath, searchPattern),
                Cron.Daily(_removeOldReportHour, _removeOldReportMin));
        }

        private static void FillMeterLife()
        {
            RecurringJob.AddOrUpdate<MeterLifeJob>(
                $"_{nameof(MeterLifeJob)}",
                job => job.RunAsync(),
                Cron.Daily(_fillMeterLifeHour),
                GetOptions());
        }
        private static RecurringJobOptions GetOptions()
        {
            var options = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };
            return options;
        }
    }
}
