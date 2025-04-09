using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestIndividualQueryService : IRequestIndividualQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividual> _requestIndividual;
        public RequestIndividualQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividual = _uow.Set<RequestIndividual>();
            _requestIndividual.NotNull(nameof(_requestIndividual));
        }

        public async Task<RequestIndividual> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestIndividual>(id);
        }

        public async Task<ICollection<RequestIndividual>> Get()
        {
            return await _requestIndividual.ToListAsync();
        }
    }
}
