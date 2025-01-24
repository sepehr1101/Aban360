using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Aban360.UserPool.Application.Features.Auth.Services.Contracts
{
    public interface ITokenValidatorService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}