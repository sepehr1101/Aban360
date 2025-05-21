using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class BankAccountQueryService : IBankAccountQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankAccount> _bankAccount;
        public BankAccountQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _bankAccount = _uow.Set<BankAccount>();
            _bankAccount.NotNull(nameof(_bankAccount));
        }

        public async Task<BankAccount> Get(short id)
        {
            return await _bankAccount
                .Include(b => b.AccountType)
                .Where(b => b.Id == id)
                .SingleAsync();//todo: type of return
        }

        public async Task<ICollection<BankAccount>> Get()
        {
            return await _bankAccount
                .Include(b => b.AccountType)
                .ToListAsync();
        }
    }
}
