using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.DbSeeder.Implementations
{
    public class ManagementSeeder : IDataSeeder
    {
        public int Order { get; set; } = 12;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingPeriodType> _readingPeriodType;
        public ManagementSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriodType=_uow.Set<ReadingPeriodType>();
            _readingPeriodType.NotNull(nameof(_readingPeriodType));
        }

        public void SeedData()
        {
            if (_readingPeriodType.Any())
            {
                return;
            }

            var filePath = GetSqlFilePath();
            _uow.ExecuteBatch(filePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\Management.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
