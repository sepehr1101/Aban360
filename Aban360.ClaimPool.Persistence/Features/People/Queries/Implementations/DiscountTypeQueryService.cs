using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class DiscountTypeQueryService : IDiscountTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DiscountType> _discountType;
        public DiscountTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _discountType = _uow.Set<DiscountType>();
            _discountType.NotNull(nameof(_discountType));
        }

        public async Task<DiscountType> Get(DiscountTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<DiscountType>(id);
        }

        public async Task<ICollection<DiscountType>> Get()
        {
            return await _discountType.ToListAsync();
        }
    }
}
