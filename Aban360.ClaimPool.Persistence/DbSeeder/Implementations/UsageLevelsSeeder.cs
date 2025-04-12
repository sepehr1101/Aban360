using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class UsageLevelsSeeder : IDataSeeder
    {
        public int Order { get; set; } = 2;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UsageLevel1> _usageLevel1;
        public UsageLevelsSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usageLevel1 = _uow.Set<UsageLevel1>();
            _usageLevel1.NotNull(nameof(_usageLevel1));
        }

        public void SeedData()
        {
            if (_usageLevel1.Any())
            {
                return;
            }
            string fileAddress = GetSqlFilePath();
            _uow.ExecuteBatch(fileAddress);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\UsageLevels.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
