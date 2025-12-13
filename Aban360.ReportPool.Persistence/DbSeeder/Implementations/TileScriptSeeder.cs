using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.DbSeeder.Implementations
{
    public class TileScriptSeeder : AbstractBaseConnection, IDataSeeder
    {
        public int Order { get; set; } = 10;

        public TileScriptSeeder(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async void SeedData()
        {
            int? recordCount = await _sqlConnection.QueryFirstOrDefaultAsync<int>(GetQuery(), null);
            if (recordCount is null || recordCount > 0)
            {
                return;
            }
            string filePath = GetSqlFilePath();
            await ExecuteBatchAsync(_sqlConnection, filePath);

        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\TileScript.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetQuery()
        {
            return @"Select COUNT(1)
                    From Aban360.ReportPool.TileScript";
        }
    }
}
