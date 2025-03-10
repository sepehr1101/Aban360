using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface ISubscriptionTypeCommandService
    {
        Task Add(SubscriptionType subscriptionType);
        Task Remove(SubscriptionType subscriptionType);
    }
}
