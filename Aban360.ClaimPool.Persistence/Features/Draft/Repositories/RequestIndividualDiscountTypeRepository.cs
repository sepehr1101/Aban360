using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Repositories
{
    internal sealed class RequestIndividualDiscountTypeRepository : IRequestIndividualDiscountTypeCommandService, IRequestIndividualDiscountTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualDiscountType> _requestIndividualDiscountType;
        public RequestIndividualDiscountTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualDiscountType = _uow.Set<RequestIndividualDiscountType>();
            _requestIndividualDiscountType.NotNull(nameof(_requestIndividualDiscountType));
        }

        public async Task Add(RequestIndividualDiscountType requestIndividualDiscountType)
        {
            await _requestIndividualDiscountType.AddAsync(requestIndividualDiscountType);
        }

        public async Task Remove(RequestIndividualDiscountType requestIndividualDiscountType)
        {
            _requestIndividualDiscountType.Remove(requestIndividualDiscountType);
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
