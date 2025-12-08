using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.TaxPool.Persistence.DbSeeder.Implementations
{
    public class MaaherErrorsSeeder : AbstractBaseConnection, IDataSeeder
    {
        public int Order { get; set; } = 30;
        public MaaherErrorsSeeder(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async void SeedData()
        {
            int? query = _sqlConnection.QueryFirstOrDefault<int>(GetQuery(), null);
            if (query != null && query.Value >= 1)
            {
                return;
            }
            string sqlFilePath = GetSqlFilePath();
            await ExecuteBatchAsync(_sqlConnection, sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\MaaherErrors.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetQuery()
        {
            return @"Select COUNT(1)
                From [Aban360].TaxPool.MaaherErrors";
        }
    }
}
