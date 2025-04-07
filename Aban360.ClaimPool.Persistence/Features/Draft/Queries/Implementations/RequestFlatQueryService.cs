using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestFlatQueryService : IRequestFlatQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestFlat> _requestFlat;
        public RequestFlatQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestFlat = _uow.Set<RequestFlat>();
            _requestFlat.NotNull(nameof(_requestFlat));
        }

        public async Task<RequestFlat> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestFlat>(id);
        }

        public async Task<ICollection<RequestFlat>> Get()
        {
            return await _requestFlat.ToListAsync();
        }
    }
}
