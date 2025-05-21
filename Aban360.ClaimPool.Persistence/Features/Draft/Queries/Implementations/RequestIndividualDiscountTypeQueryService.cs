using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestIndividualDiscountTypeQueryService : IRequestIndividualDiscountTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualDiscountType> _requestIndividualDiscountType;
        public RequestIndividualDiscountTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualDiscountType = _uow.Set<RequestIndividualDiscountType>();
            _requestIndividualDiscountType.NotNull(nameof(_requestIndividualDiscountType));
        }

        public async Task<RequestIndividualDiscountType> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestIndividualDiscountType>(id);
        }
        
        public async Task<ICollection<RequestIndividualDiscountType>> GetByIndividualId(int id)
        {
            return await _requestIndividualDiscountType
                .Where(x => x.IndividualId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RequestIndividualDiscountType>> Get()
        {
            return await _requestIndividualDiscountType.ToListAsync();
        }
    }
}
