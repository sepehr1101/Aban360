using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class TokenValidatorHandler : ITokenValidatorHandler
    {
        public Task ValidateAsync(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }
    }
}
