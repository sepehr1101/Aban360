using Aban360.ClaimPool.Domain.Features.Registration.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Queries.Implementations
{
    public class SubscriptionQueryService : ISubscriptionQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Subscription> _subscription;
        public SubscriptionQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscription = _uow.Set<Subscription>();
            _subscription.NotNull(nameof(_subscription));
        }

        public async Task<Subscription> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Subscription>(id);
        }

        public async Task<ICollection<Subscription>> Get()
        {
            return await _subscription.ToListAsync();
        }
    }
}
