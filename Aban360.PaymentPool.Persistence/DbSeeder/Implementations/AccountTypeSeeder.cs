using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class AccountTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AccountType> _accountType;
        public AccountTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _accountType = _uow.Set<AccountType>();
            _accountType.NotNull(nameof(_accountType));
        }

        public void SeedData()
        {
            if (_accountType.Any())
            {
                return;
            }

            ICollection<AccountType> accountTypes = new List<AccountType>()
            {
                new AccountType(){Id=AccountTypeEnum.WaterBill,Title="آب بها"},
                new AccountType(){Id=AccountTypeEnum.Branch,Title="حق انشعاب"},
                new AccountType(){Id=AccountTypeEnum.Note,Title="تبصره"},
            };
            _accountType.AddRange(accountTypes);
            _uow.SaveChanges();
        }
    }
}
