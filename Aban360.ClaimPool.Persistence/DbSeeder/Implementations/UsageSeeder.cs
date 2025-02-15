using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class UsageSeeder : IDataSeeder
    {
        public int Order { get; set; } = 11;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Usage> _usages;
        public UsageSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _usages = _uow.Set<Usage>();
            _usages.NotNull(nameof(_usages));
        }

        public void SeedData()
        {
            if (_usages.Any())
            {
                return;
            }
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
