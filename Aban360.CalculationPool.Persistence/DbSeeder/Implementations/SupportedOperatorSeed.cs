using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class SupportedOperatorSeed: IDataSeeder
    {
        public int Order { get; set; } = 45;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SupportedOperator> _supportedOperators;
        public SupportedOperatorSeed(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _supportedOperators=_uow.Set<SupportedOperator>();
            _supportedOperators.NotNull(nameof(_supportedOperators));
        }

        public void SeedData()
        {
            if (_supportedOperators.Any())
            {
                return;
            }

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\supportedOperation.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
