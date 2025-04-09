using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestEstateQueryService : IRequestEstateQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestEstate> _requestEstate;
        public RequestEstateQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestEstate = _uow.Set<RequestEstate>();
            _requestEstate.NotNull(nameof(_requestEstate));
        }

        public async Task<RequestEstate> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestEstate>(id);
        }

        public async Task<ICollection<RequestEstate>> Get()
        {
            return await _requestEstate.ToListAsync();
        }
    }
}
