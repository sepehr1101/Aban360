using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aban360.UserPool.Application.Features.Auth.Services.Implementations
{
    public class TokenFactoryService : ITokenFactoryService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptionsSnapshot<BearerTokenOptions> _configuration;
        private readonly IUserRoleQueryService _userRoleQueryService;
        private readonly ILogger<TokenFactoryService> _logger;

        public TokenFactoryService(
            IHttpContextAccessor contextAccessor,
            IUserRoleQueryService userRoleQueryService,
            IOptionsSnapshot<BearerTokenOptions> configuration,
            ILogger<TokenFactoryService> logger)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(_userRoleQueryService));

            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));

            _logger = logger;
            _logger.NotNull(nameof(logger));
        }

        public async Task<JwtTokenData> CreateJwtTokensAsync(User user)
        {
            var userRoles = await _userRoleQueryService.GetIncludeRoles(user.Id);
            var roles = userRoles.Select(ur => ur.Role)?.ToList();
            var (accessToken, claims) = await CreateAccessTokenAsync(user, roles);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokenData
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) CreateRefreshToken()
        {
            var refreshTokenSerial = SecurityOperations.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, SecurityOperations.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _configuration.Value.Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                signingCredentials: creds);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key)),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                    out _
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

        private async Task<(string AccessToken, IEnumerable<Claim> Claims)> CreateAccessTokenAsync(User user, ICollection<Role>? roles)
        {
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokens
                new Claim(JwtRegisteredClaimNames.Jti, SecurityOperations.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String, _configuration.Value.Issuer),
                //new Claim(CustomClaimTypes.DisplayName, user.DisplayName, ClaimValueTypes.String, _configuration.Value.Issuer),               
                // to invalidate the cookie
                new Claim(ClaimTypes.SerialNumber, user.SerialNumber, ClaimValueTypes.String, _configuration.Value.Issuer),
                // custom data
                new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // device detection in token
                new Claim(ClaimTypes.System, await DeviceDetection.GetHash(_contextAccessor.HttpContext.Request), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.GivenName, user.FullName, ClaimValueTypes.String, _configuration.Value.Issuer)
            };

            // add roles
            if (roles is not null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name, ClaimValueTypes.String, _configuration.Value.Issuer));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}