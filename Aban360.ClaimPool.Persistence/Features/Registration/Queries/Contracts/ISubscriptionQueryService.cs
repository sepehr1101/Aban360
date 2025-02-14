using Aban360.ClaimPool.Domain.Features.Registration.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts
{
    public interface ISubscriptionQueryService
    {
        Task<Subscription> Get(int id);
        Task<ICollection<Subscription>> Get();
    }
}
