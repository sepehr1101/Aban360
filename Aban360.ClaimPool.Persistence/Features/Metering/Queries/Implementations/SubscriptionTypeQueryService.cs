using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class SubscriptionTypeQueryService : ISubscriptionTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SubscriptionType> _subscriptionType;
        public SubscriptionTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _subscriptionType = _uow.Set<SubscriptionType>();
            _subscriptionType.NotNull(nameof(_subscriptionType));
        }

        public async Task<SubscriptionType> Get(SubscriptionTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<SubscriptionType>(id);
        }

        public async Task<ICollection<SubscriptionType>> Get()
        {
            return await _subscriptionType.ToListAsync();
        }
    }
}
