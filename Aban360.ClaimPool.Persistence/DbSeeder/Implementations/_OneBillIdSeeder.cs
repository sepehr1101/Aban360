using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class _OneBillIdSeeder //: IDataSeeder
    {
        public int Order { get; set; } = 40;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Estate> _estate;
        public _OneBillIdSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _estate = _uow.Set<Estate>();
            _estate.NotNull(nameof(_estate));
        }

        public void SeedData()
        {
            if (_estate.Any())
            {
                return;
            }
            string fileAddress = GetSqlFilePath();
            _uow.ExecuteBatch(fileAddress);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\seed-manual-one-billid.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
