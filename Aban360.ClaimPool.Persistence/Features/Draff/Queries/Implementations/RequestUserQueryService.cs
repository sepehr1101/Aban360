using Aban360.ClaimPool.Domain.Features.Draff.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draff.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draff.Queries.Implementations
{
    internal sealed class RequestUserQueryService : IRequestUserQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestUser> _requestUser;
        public RequestUserQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestUser = _uow.Set<RequestUser>();
            _requestUser.NotNull(nameof(_requestUser));
        }

        public async Task<RequestUser> Get(short id)
        {
            return await _uow.FindOrThrowAsync<RequestUser>(id);
        }

        public async Task<ICollection<RequestUser>> Get()
        {
            return await _requestUser.ToListAsync();
        }
    }
}
