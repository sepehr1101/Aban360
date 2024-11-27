using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ITokenValidatorHandler
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}