using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.SystemPool.Persistence.DbSeeder.Implementations
{
    public class FaqSeeder : AbstractBaseConnection, IDataSeeder
    {
        public int Order { get; set; } = 10;
        public FaqSeeder(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async void SeedData()
        {
            int? faqRecordCount = await _sqlConnection.QueryFirstOrDefaultAsync<int>(GetQuery(), null);
            if (faqRecordCount != null && faqRecordCount >= 1)
            {
                return;
            }
            string sqlFilePath = GetSqlFilePath();
            await ExecuteBatchAsync(_sqlReportConnection, sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\Faq.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetQuery()
        {
            return @"Select COUNT(1)
                    From [Aban360].UserGuidePool.Faq";
        }
    }
}
