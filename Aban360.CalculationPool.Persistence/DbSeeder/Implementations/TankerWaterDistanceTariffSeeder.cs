using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class TankerWaterDistanceTariffSeeder : AbstractBaseConnection, IDataSeeder
    {
        public int Order { get; set; } = 10;

        public TankerWaterDistanceTariffSeeder(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async void SeedData()
        {
            int? count = await _sqlConnection.QueryFirstOrDefaultAsync<int>(GetCountQuery(), null);
            if (count is not null && count > 0)
            {
                return;
            }
            string sqlFilePath = GetSqlFilePath();
            await ExecuteBatchAsync(_sqlConnection, sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\TankerWaterDistnaceTariff.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
        private string GetCountQuery()
        {
            return @"Select COUNT(1)
                    From Aban360.CalculationPool.TankerWaterDistanceTariff
                    Where 
                    	RemoveByUserId IS NULL AND
                    	RemoveDateTime IS NULL";
        }
    }
}
