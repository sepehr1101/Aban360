using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

namespace Aban360.PaymentPool.Persistence.Features.Queries.Contracts
{
    public interface IPaymentMethodQueryService
    {
        Task<PaymentMethod> Get(PaymentMethodEnum id);
        Task<ICollection<PaymentMethod>> Get();
    }
}
