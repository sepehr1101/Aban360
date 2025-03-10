using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    public class SubscriptionTypeCommandService : ISubscriptionTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SubscriptionType> _subscriptionType;
        public SubscriptionTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _subscriptionType = _uow.Set<SubscriptionType>();
            _subscriptionType.NotNull(nameof(_subscriptionType));
        }

        public async Task Add(SubscriptionType subscriptionType)
        {
            await _subscriptionType.AddAsync(subscriptionType);
        }

        public async Task Remove(SubscriptionType subscriptionType)
        {
            _subscriptionType.Remove(subscriptionType);
        }
    }
}
