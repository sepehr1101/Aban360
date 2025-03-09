using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class SubscriptionTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SubscriptionType> _subscriptionType;
        public SubscriptionTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _subscriptionType = _uow.Set<SubscriptionType>();
            _subscriptionType.NotNull(nameof(_subscriptionType));
        }

        public void SeedData()
        {
            if (_subscriptionType.Any())
            {
                return;
            }

            ICollection<SubscriptionType> SubscriptionType = new List<SubscriptionType>()
            {
                new SubscriptionType(){Id=SubscriptionTypeEnum.Temporary,Title="موقت"},
                new SubscriptionType(){Id=SubscriptionTypeEnum.Permanent,Title="دائم"}// todo:Change Title
            };
            _subscriptionType.AddRange(SubscriptionType);
            _uow.SaveChanges();
        }
    }
}
