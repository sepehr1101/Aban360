using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Implementations
{
    internal sealed class BankQueryService : IBankQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Bank> _paymentProcedure;
        public BankQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _paymentProcedure = _uow.Set<Bank>();
            _paymentProcedure.NotNull(nameof(_paymentProcedure));
        }

        public async Task<Bank> Get(short id)
        {
            return await _uow.FindOrThrowAsync<Bank>(id);
        }

        public async Task<ICollection<Bank>> Get()
        {
            return await _paymentProcedure.ToListAsync();
        }
    }
}
