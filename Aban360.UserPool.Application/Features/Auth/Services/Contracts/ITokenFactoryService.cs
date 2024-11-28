using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Services.Contracts
{
    public interface ITokenFactoryService
    {
        Task<JwtTokenData> CreateJwtTokensAsync(User user);
        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}