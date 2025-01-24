using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Aban360.UserPool.Application.Features.Auth.Services.Implementations
{
    public class TokenValidatorService : ITokenValidatorService
    {
        public Task ValidateAsync(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }
    }
}
