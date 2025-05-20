using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestIndividualTagQueryService : IRequestIndividualTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualTag> _requestIndividualTag;
        public RequestIndividualTagQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualTag = _uow.Set<RequestIndividualTag>();
            _requestIndividualTag.NotNull(nameof(_requestIndividualTag));
        }

        public async Task<RequestIndividualTag> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestIndividualTag>(id);
        }
        public async Task<ICollection<RequestIndividualTag>> GetByIndividualId(int id)
        {
            return await _requestIndividualTag
                .Where(x => x.IndividualId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RequestIndividualTag>> Get()
        {
            return await _requestIndividualTag.ToListAsync();
        }
    }
}
