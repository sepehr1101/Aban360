using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class ServicesSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Offering> _offering;

        public ServicesSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _offering=_uow.Set<Offering>();
            _offering.NotNull(nameof(_offering));
        }
        public void SeedData()
        {
            if (_offering.Any())
            {
                return;
            }

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\Services.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
