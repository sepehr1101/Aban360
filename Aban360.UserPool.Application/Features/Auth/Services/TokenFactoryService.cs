using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.UserPool.Application.Features.Auth.Services
{
    public class TokenFactoryService 
    {       
        private readonly IOptionsSnapshot<BearerTokenOptions> _configuration;
        private readonly IRoleQueryService _rolesService;
        private readonly ILogger<TokenFactoryService> _logger;

        public TokenFactoryService(
            IRoleQueryService rolesService,
            IOptionsSnapshot<BearerTokenOptions> configuration,
            ILogger<TokenFactoryService> logger)
        {
            _rolesService = rolesService;
            _rolesService.NotNull(nameof(rolesService));

            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));

            _logger = logger;
            _logger.NotNull(nameof(logger));
        }

        public async Task<JwtTokenData> CreateJwtTokensAsync(User user)
        {
            var (accessToken, claims) = await CreateAccessTokenAsync(user);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokenData
            {
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

        //private async Task<(string AccessToken, IEnumerable<Claim> Claims)> CreateAccessTokenAsync(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        // Unique Id for all Jwt tokens
        //        new Claim(JwtRegisteredClaimNames.Jti, SecurityOperations.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
        //        // Issuer
        //        new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
        //        // Issued at
        //        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
        //        new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String, _configuration.Value.Issuer),
        //        new Claim(CustomClaimTypes.DisplayName, user.DisplayName, ClaimValueTypes.String, _configuration.Value.Issuer),               
        //        // to invalidate the cookie
        //        new Claim(ClaimTypes.SerialNumber, user.SerialNumber, ClaimValueTypes.String, _configuration.Value.Issuer),
        //        // custom data
        //        new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
        //        // device detection in token
        //        new Claim(ClaimTypes.System, Detec.GetCurrentRequestDeviceDetailsHash(), ClaimValueTypes.String, _configuration.Value.Issuer),
        //    };

        //    // add roles
        //    var roles = await _rolesService.FindUserRolesAsync(user.Id);
        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role.Title, ClaimValueTypes.String, _configuration.Value.Issuer));
        //    }

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var now = DateTime.UtcNow;
        //    var token = new JwtSecurityToken(
        //        issuer: _configuration.Value.Issuer,
        //        audience: _configuration.Value.Audience,
        //        claims: claims,
        //        notBefore: now,
        //        expires: now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
        //        signingCredentials: creds);
        //    return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        //}
    }
}
