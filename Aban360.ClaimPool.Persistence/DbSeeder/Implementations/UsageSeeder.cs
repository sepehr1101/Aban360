using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class UsageSeeder : IDataSeeder
    {
        public int Order { get; set; } = 12;
        private readonly IUnitOfWork _uow;
        public UsageSeeder( IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
        }

        public void SeedData()
        {
            string fileAddress = GetSqlFilePath();
            _uow.ExecuteBatch(fileAddress);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\Usage.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
