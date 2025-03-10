using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface ISubscriptionTypeQueryService
    {
        Task<SubscriptionType> Get(SubscriptionTypeEnum id);
        Task<ICollection<SubscriptionType>> Get();
    }
}
