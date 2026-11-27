using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class BankAccountSeeder:IDataSeeder
    {
        public int Order { get; set; } = 11;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankAccount> _bankAccount;
        public BankAccountSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankAccount = _uow.Set<BankAccount>();
            _bankAccount.NotNull(nameof(_bankAccount));
        }

        public void SeedData()
        {
            if (_bankAccount.Any())
            {
                return;
            }
            string fileAddress = GetSqlFilePath();
            _uow.ExecuteBatch(fileAddress);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\BankAccount.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
