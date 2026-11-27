using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Implementations
{
    internal sealed class AccountTypeCommandService : IAccountTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AccountType> _accountType;
        public AccountTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _accountType = _uow.Set<AccountType>();
            _accountType.NotNull(nameof(_accountType));
        }

        public async Task Add(AccountType accountType)
        {
            await _accountType.AddAsync(accountType);
        }

        public async Task Remove(AccountType accountType)
        {
            _accountType.Remove(accountType);
        }
    }
}
