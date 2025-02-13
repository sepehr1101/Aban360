using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Aban360.UserPool.Application.Features.Auth.Services.Implementations
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IUserQueryService _userQueryService;
        private readonly ITokenStoreQueryService _tokenStoreQueryService;
        private readonly ITokenFailureTypeQueryService _tokenFailureTypeQueryService;

        public TokenValidatorService(
            IUserQueryService userQueryService,
            ITokenStoreQueryService tokenStoreQueryService,
            ITokenFailureTypeQueryService tokenFailureTypeQueryService)
        {
            _userQueryService= userQueryService;
            _userQueryService.NotNull(nameof(_userQueryService));

            _tokenStoreQueryService= tokenStoreQueryService;
            _tokenStoreQueryService.NotNull(nameof(_tokenStoreQueryService));

            _tokenFailureTypeQueryService= tokenFailureTypeQueryService;
            _tokenFailureTypeQueryService.NotNull(nameof(tokenFailureTypeQueryService));
        }
        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var failureReason = await GetFailureType(context);
            if (failureReason is not null)
            {
                var tokenFailureType= await _tokenFailureTypeQueryService.Get(failureReason.Value);
                context.Fail(tokenFailureType.Title);
            }
            return;
        }
        private string? GetClientIP(TokenValidatedContext context)
        {
            string? ip = context?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return ip;
        }
        private (string?, string?) GetControllerAction(TokenValidatedContext context)
        {
            string? controller = (string?)context?.HttpContext?.Request?.RouteValues?["controller"];
            string? action = (string?)context?.HttpContext?.Request?.RouteValues?["action"];
            return (controller, action);
        }
        private async Task<TokenFailureTypeEnum?> GetFailureType(TokenValidatedContext context)
        {
            ClaimsPrincipal? claimsPrinciple = context?.Principal;
            TokenFailureTypeEnum? failureReasonId = null;
            if (claimsPrinciple is null || claimsPrinciple.Claims is null || !claimsPrinciple.Claims.Any())
            {
                failureReasonId = TokenFailureTypeEnum.NoClaims;
                return failureReasonId;
            }

            var (controller, action) = GetControllerAction(context);
            if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
            {               
                failureReasonId = TokenFailureTypeEnum.NoActionOrController;
                return failureReasonId;
            }

            
            var serialNumberClaim = claimsPrinciple.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {               
                failureReasonId = TokenFailureTypeEnum.NoSerial;
                return (failureReasonId);
            }

            var userIdString = claimsPrinciple.FindFirst(ClaimTypes.UserData);

            if (userIdString is null)
            {
                failureReasonId = TokenFailureTypeEnum.NoUserId;
                return failureReasonId;
            }

            Guid userId;
            var parseResult= Guid.TryParse(userIdString.Value, out userId);
            if (!parseResult)
            {               
                failureReasonId = TokenFailureTypeEnum.NoUserId;
                return failureReasonId;
            }            
           
            var user = await _userQueryService.Get(userId);
            if (user == null || user.SerialNumber != serialNumberClaim.Value || user.ValidTo!=null)
            {               
                failureReasonId = TokenFailureTypeEnum.Expired;
                return failureReasonId;
            }

            if (context.SecurityToken is not JsonWebToken accessToken)
            {
                failureReasonId = TokenFailureTypeEnum.NoTokenInDb;
                return failureReasonId;
            }

            //var accessToken = context.SecurityToken as JwtSecurityToken;
            //if (string.IsNullOrWhiteSpace(accessToken.RawData) ||
            //    !await _tokenStoreQueryService.IsValid(accessToken.RawData, userId))
            //{               
            //    failureReasonId = TokenFailureTypeEnum.NoTokenInDb;
            //    return failureReasonId;
            //}

            //if (!_deviceDetectionService.HasUserTokenValidDeviceDetails(claimsIdentity))
            //{               
            //    failureReasonId = TokenFailureTypeEnum.DeviceChanged;
            //    return (failureReasonId);
            //}

            //var isSessionClosed = await _usersService.UpdateUserLastActivityDateAsync(userId);
            //if (isSessionClosed)
            //{                    
            //    failureReasonId = TokenFailureTypeEnum.InactiveSession;
            //    return (failureReasonId);
            //}

            return (failureReasonId);
        }
    }
}
