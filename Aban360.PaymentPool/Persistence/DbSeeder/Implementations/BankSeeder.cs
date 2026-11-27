
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class BankSeeder:IDataSeeder
    {
        public int Order { get; set; } = 9;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Bank> _bank;
        public BankSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bank = _uow.Set<Bank>();
            _bank.NotNull(nameof(_bank));
        }

        public void SeedData()
        {
            if (_bank.Any())
            {
                return;
            }
            string fileAddress = GetSqlFilePath();
            _uow.ExecuteBatch(fileAddress);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\Bank.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
