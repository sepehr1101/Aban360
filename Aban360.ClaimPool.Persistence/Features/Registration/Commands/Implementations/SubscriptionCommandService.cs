using Aban360.ClaimPool.Domain.Features.Registration.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Registration.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Commands.Implementations
{
    public class SubscriptionCommandService : ISubscriptionCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Subscription> _subscription;
        public SubscriptionCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscription = _uow.Set<Subscription>();
            _subscription.NotNull(nameof(_subscription));
        }

        public async Task Add(Subscription subscription)
        {
            await _subscription.AddAsync(subscription);
        }

        public async Task Remove(Subscription subscription)
        {
            _subscription.Remove(subscription);
        }
    }
}
