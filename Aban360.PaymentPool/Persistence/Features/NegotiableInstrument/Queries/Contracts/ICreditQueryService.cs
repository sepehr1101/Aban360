using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts
{
    public interface ICreditQueryService
    {
        Task<Credit> Get(long id);
        Task<ICollection<Credit>> Get();
    }
}
