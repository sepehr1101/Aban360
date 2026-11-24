using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Repositories
{
    internal sealed class RequestSiphonRepository : IRequestSiphonCommandService, IRequestSiphonQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestSiphon> _requestSiphon;
        public RequestSiphonRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestSiphon = _uow.Set<RequestSiphon>();
            _requestSiphon.NotNull(nameof(_requestSiphon));
        }

        public async Task Add(RequestSiphon requestSiphon)
        {
            await _requestSiphon.AddAsync(requestSiphon);
        }

        public void Remove(RequestSiphon requestSiphon)
        {
            _requestSiphon.Remove(requestSiphon);
        }

        public async Task<RequestSiphon> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestSiphon>(id);
        }

        public async Task<ICollection<RequestSiphon>> Get()
        {
            return await _requestSiphon.ToListAsync();
        }

    }
}
