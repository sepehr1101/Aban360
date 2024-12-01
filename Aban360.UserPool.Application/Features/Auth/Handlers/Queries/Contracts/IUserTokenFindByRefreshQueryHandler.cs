using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserTokenFindByRefreshQueryHandler
    {
        Task<UserToken?> Handle(RefreshToken refreshTokenDto, CancellationToken cancellationToken);
    }
}