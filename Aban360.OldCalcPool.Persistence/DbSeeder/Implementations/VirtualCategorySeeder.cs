using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.DbSeeder.Implementations
{
    public class VirtualCategorySeeder : AbstractBaseConnection, IDataSeeder
    {
        public int Order { get; set; } = 10;

        public VirtualCategorySeeder(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async void SeedData()
        {
            int? query = _sqlReportConnection.QueryFirstOrDefault<int>(GetQuery(), null);
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
            string relativePath = @"\DbSeeder\DataScript\VirtualCategory.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetQuery()
        {
            return @"Select COUNT(1)
                    From [Db70].dbo.VirtualCategory";
        }
    }
}
