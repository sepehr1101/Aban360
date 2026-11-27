using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Implementations
{
    internal sealed class CreditCommandService : ICreditCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Credit> _credit;
        public CreditCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _credit = _uow.Set<Credit>();
            _credit.NotNull(nameof(_credit));
        }

        public async Task Add(Credit credit)
        {
            await _credit.AddAsync(credit);
        }
        public async Task Add(ICollection<Credit> credit)
        {
            await _credit.AddRangeAsync(credit);
        }

        public async Task Remove(Credit credit)
        {
            _credit.Remove(credit);
        }
    }
}
