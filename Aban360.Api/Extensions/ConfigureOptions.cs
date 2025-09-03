using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.Api.Extensions
{
    public static class ConfigureOptions
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBearerTokens(configuration);
            services.AddApiSettings(configuration);
            services.AddOpenKm(configuration);
            return services;
        }

        //TODO: remove Validate method and invoke it somewhere else
        private static void AddBearerTokens(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BearerTokenOptions>()
                .Bind(configuration.GetSection("BearerTokens"))
                .Validate(bearerTokens =>
                {
                    return bearerTokens.AccessTokenExpirationMinutes < bearerTokens.RefreshTokenExpirationMinutes;
                }, MessageResources.RefreshTokenIsLessThanToken);
        }
        private static void AddApiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ApiSettings>().Bind(configuration.GetSection("ApiSettings"));
        }
        private static void AddOpenKm(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenKmOptions>(configuration.GetSection(OpenKmOptions.SectionName));
        }
    }
}
