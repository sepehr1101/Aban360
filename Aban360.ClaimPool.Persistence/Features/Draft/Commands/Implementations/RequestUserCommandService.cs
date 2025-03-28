using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestUserCommandService : IRequestUserCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestUser> _requestUser;
        public RequestUserCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestUser = _uow.Set<RequestUser>();
            _requestUser.NotNull(nameof(_requestUser));
        }

        public async Task Add(RequestUser requestUser)
        {
            await _requestUser.AddAsync(requestUser);
        }
    }
}
