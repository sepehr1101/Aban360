using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Implementations
{
    internal sealed class PaymentMethodQueryService : IPaymentMethodQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PaymentMethod> _paymentMethod;
        public PaymentMethodQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _paymentMethod = _uow.Set<PaymentMethod>();
            _paymentMethod.NotNull(nameof(_paymentMethod));
        }

        public async Task<PaymentMethod> Get(PaymentMethodEnum id)
        {
            return await _uow.FindOrThrowAsync<PaymentMethod>(id);
        }

        public async Task<ICollection<PaymentMethod>> Get()
        {
            return await _paymentMethod.ToListAsync();
        }
    }
}
