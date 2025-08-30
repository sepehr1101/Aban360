using Aban360.Api.Cronjobs;
using Hangfire;

namespace Aban360.Api.Extensions
{
    internal static class ConfigureCronjobs
    {
        internal static void AddCronjobs(this IConfiguration configuration)
        {
            RemoveOldReports(configuration);
        }
        private static void RemoveOldReports(IConfiguration configuration)
        {
            int tresholdDay = int.Parse(configuration["FileManagement:ExcelExpireDay"]);
            string excelFilePath = configuration["FileManagement:ExcelPath"].ToString();
            string searchPattern = "*.xlsx";//todo: get from config
            int utcHour = 2, utcMinute = 45;//todo: get from config
            RecurringJob.AddOrUpdate("RemoveOldReports", () => FileRemover.DeleteOldFiles(tresholdDay, excelFilePath, searchPattern), Cron.Daily(utcHour, utcMinute));
        }
    }
}
