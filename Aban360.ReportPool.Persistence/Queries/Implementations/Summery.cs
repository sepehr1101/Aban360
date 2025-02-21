using Dapper;
using Microsoft.Data.SqlClient;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface ISummery
    {
        Task<ResultSummeryDto> GetSummery(string id);
    }

    public class Summery : ISummery
    {
        public Summery()
        {
        }
        public async Task<ResultSummeryDto> GetSummery(string id)
        {
            var connection = new SqlConnection("Data Source=.;Encrypt=False;Database=Aban360;Integrated Security=true;TrustServerCertificate=true;");
            string fileAddress = GetSqlFilePath();
            string sql = File.ReadAllText(fileAddress);

            var result = connection.QuerySingle<ResultSummeryDto>(sql, new { id = id });
            var x = result;
            return x;
        }

        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\Queries\Query\SummeryQuery.txt";

            var path = string.Concat(basePath, relativePath);
            return path;
        }

    }
    public record ResultSummeryDto
    {
        int WaterMeterId;
        string? BillId;
        string? ReadingNumber;
        string? Address;
        string? ConstructionType;
        string? UsageConsumtion;
        string? UsageSell;
        string? FullName;
        string? WaterMeterTag;
        DateTime? InstallationDate;
        DateTime? ProductDate;
        DateTime? GuaranteeDate;
        DateTime? SiphonInstallationDate;
    }
}