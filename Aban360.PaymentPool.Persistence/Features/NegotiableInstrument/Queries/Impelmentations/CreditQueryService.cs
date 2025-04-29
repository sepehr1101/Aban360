using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class CreditQueryService : ICreditQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Credit> _credit;
        public CreditQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _credit = _uow.Set<Credit>();
            _credit.NotNull(nameof(_credit));
        }

        public async Task<Credit> Get(long id)
        {
            return await _uow.FindOrThrowAsync<Credit>(id);
        }

        public async Task<ICollection<Credit>> Get()
        {
            return await _credit.ToListAsync();
        }
    }
}
