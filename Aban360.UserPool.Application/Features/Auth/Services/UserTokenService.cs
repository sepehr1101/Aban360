using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IUserTokenService
{
    Task CreateAsync(JwtTokenData jwtTokenData, string? refreshTokenSourceSerial, CancellationToken cancellationToken);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserToken?> FindByRefreshAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}

internal sealed class UserTokenService : IUserTokenService
{
    private readonly ITokenStoreCommandService _tokeStoreService;
    private readonly IMapper _mapper;
    private readonly IOptionsSnapshot<BearerTokenOptions> _configuration;
    private readonly ITokenStoreQueryService _tokenStoreService;
    private readonly ITokenFactoryService _tokenFactoryService;
    public UserTokenService(ITokenStoreCommandService tokenStoreService, IMapper mapper, IOptionsSnapshot<BearerTokenOptions> configuration, ITokenStoreQueryService findByRefreshAsyncTokenStoreService, ITokenFactoryService tokenFactoryService)
    {
        _tokeStoreService = tokenStoreService;
        _tokeStoreService.NotNull(nameof(tokenStoreService));
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _configuration = configuration;
        _configuration.NotNull(nameof(configuration));
        _tokenStoreService = findByRefreshAsyncTokenStoreService;
        _tokenStoreService.NotNull(nameof(findByRefreshAsyncTokenStoreService));
        _tokenFactoryService = tokenFactoryService;
        _tokenFactoryService.NotNull(nameof(tokenFactoryService));
    }

    public async Task CreateAsync(JwtTokenData jwtTokenData, string? refreshTokenSourceSerial, CancellationToken cancellationToken)
    {
        UserToken userToken = await CreateCreateUserToken(jwtTokenData, refreshTokenSourceSerial);
        await _tokeStoreService.RemoveTokensWithSameRefreshTokenSource(jwtTokenData.UserId);
        await _tokeStoreService.Add(userToken);
    }

    private async Task<UserToken> CreateCreateUserToken(JwtTokenData jwtTokenData, string? refreshTokenSourceSerial)
    {
        DateTime now = DateTime.Now;
        UserToken userToken = new UserToken
        {
            UserId = jwtTokenData.UserId,
            // Refresh token handles should be treated as secrets and should be stored hashed
            RefreshTokenIdHash = await SecurityOperations.GetSha256Hash(jwtTokenData.RefreshTokenSerial),
            RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial) ? null : await SecurityOperations.GetSha256Hash(refreshTokenSourceSerial),
            AccessTokenHash = await SecurityOperations.GetSha256Hash(jwtTokenData.AccessToken),
            RefreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
            AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes)
        };
        return userToken;
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _tokeStoreService.Remove(userId);
    }

    public async Task<UserToken?> FindByRefreshAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        //validate refreshtokendto
        string refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshToken.Value);
        if (string.IsNullOrWhiteSpace(refreshTokenSerial))
        {
            return default;
        }

        string refreshTokenHash = await SecurityOperations.GetSha256Hash(refreshTokenSerial);
        UserToken userToken = await _tokenStoreService.Get(refreshTokenHash);
        return userToken;
    }
}
