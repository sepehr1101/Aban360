using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Implementations
{
    internal sealed class BankAccountCommandService : IBankAccountCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankAccount> _bankAccount;
        public BankAccountCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _bankAccount = _uow.Set<BankAccount>();
            _bankAccount.NotNull(nameof(_bankAccount));
        }

        public async Task Add(BankAccount BankAccount)
        {
            await _bankAccount.AddAsync(BankAccount);
        }

        public async Task Remove(BankAccount BankAccount)
        {
            _bankAccount.Remove(BankAccount);
        }
    }
}
