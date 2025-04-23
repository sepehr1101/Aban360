using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

namespace Aban360.PaymentPool.Persistence.Features.Queries.Contracts
{
    public interface IBankQueryService
    {
        Task<Bank> Get(short id);
        Task<ICollection<Bank>> Get();
    }
}
