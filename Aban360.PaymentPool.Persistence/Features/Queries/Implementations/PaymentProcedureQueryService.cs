using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.Queries.Implementations
{
    internal sealed class PaymentProcedureQueryService : IPaymentProcedureQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PaymentProcedure> _paymentProcedure;
        public PaymentProcedureQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _paymentProcedure = _uow.Set<PaymentProcedure>();
            _paymentProcedure.NotNull(nameof(_paymentProcedure));
        }

        public async Task<PaymentProcedure> Get(PaymentProcedureEnum id)
        {
            return await _uow.FindOrThrowAsync<PaymentProcedure>(id);
        }

        public async Task<ICollection<PaymentProcedure>> Get()
        {
            return await _paymentProcedure.ToListAsync();
        }
    }
}
