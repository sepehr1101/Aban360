using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts
{
    public interface IUserTokenCreateHandler
    {
        Task Handle(JwtTokenData jwtTokenData, string? refreshTokenSourceSerial, CancellationToken cancellationToken);
    }
}