using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class AccountTypeQueryService : IAccountTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AccountType> _accountType;
        public AccountTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _accountType = _uow.Set<AccountType>();
            _accountType.NotNull(nameof(_accountType));
        }

        public async Task<AccountType> Get(AccountTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<AccountType>(id);
        }

        public async Task<ICollection<AccountType>> Get()
        {
            return await _accountType.ToListAsync();
        }
    }
}
