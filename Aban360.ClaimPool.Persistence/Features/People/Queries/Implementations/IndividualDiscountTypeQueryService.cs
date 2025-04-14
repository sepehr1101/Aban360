using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class IndividualDiscountTypeQueryService : IIndividualDiscountTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualDiscountType> _individualDiscountType;
        public IndividualDiscountTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualDiscountType = _uow.Set<IndividualDiscountType>();
            _individualDiscountType.NotNull(nameof(_individualDiscountType));
        }

        public async Task<IndividualDiscountType> Get(int id)
        {
            return await _uow.FindOrThrowAsync<IndividualDiscountType>(id);
        }

        public async Task<ICollection<IndividualDiscountType>> Get()
        {
            return await _individualDiscountType.ToListAsync();
        }
    }
}
