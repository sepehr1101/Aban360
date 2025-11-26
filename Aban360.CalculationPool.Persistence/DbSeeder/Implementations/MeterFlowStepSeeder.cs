using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class MeterFlowStepSeeder : AbstractBaseConnection, IDataSeeder
    {
        public MeterFlowStepSeeder(IConfiguration configuration)
            : base(configuration)
        {
        }

        public int Order { get; set; } = 10;

        public async void SeedData()
        {
            int? query = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(GetQuery(), null);
            if (query != null && query.Value >= 1)
            {
                return;
            }
            string sqlFilePath = GetSqlFilePath();
            await ExecuteBatchAsync(_sqlReportConnection, sqlFilePath);
        }

        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\MeterFlowStep.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetQuery()
        {
            return @"Select Count(1)
                    From [Atlas].dbo.MeterFlowStep";
        }
    }
}
