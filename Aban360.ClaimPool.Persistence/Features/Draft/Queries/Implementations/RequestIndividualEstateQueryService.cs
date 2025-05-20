using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestIndividualEstateQueryService : IRequestIndividualEstateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualEstate> _requestIndividualEstate;
        public RequestIndividualEstateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualEstate = _uow.Set<RequestIndividualEstate>();
            _requestIndividualEstate.NotNull(nameof(_requestIndividualEstate));
        }

        public async Task<RequestIndividualEstate> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestIndividualEstate>(id);
        }
        
        public async Task<RequestIndividualEstate> GetByIndividualId(int id)
        {
            return await _requestIndividualEstate
                .Where(i => i.IndividualId== id)
                .SingleOrDefaultAsync();
        }

        public async Task<ICollection<RequestIndividualEstate>> Get()
        {
            return await _requestIndividualEstate.ToListAsync();
        }
    }
}
