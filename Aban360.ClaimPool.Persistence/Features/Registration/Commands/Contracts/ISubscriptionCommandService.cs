using Aban360.ClaimPool.Domain.Features.Registration.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Commands.Contracts
{
    public interface ISubscriptionCommandService
    {
        Task Add(Subscription subscription);
        Task Remove(Subscription subscription);
    }
}
