using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Authentication;

namespace Aban360.BrdigeApi.Extensions
{
    public static class ConfigureOptions
    {
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            AddBearerTokens(services, configuration);
            AddApiSettings(services, configuration);
            AddOpenKm(services, configuration);
            AddEsbAuthentication(services, configuration);
        }
        private static void AddBearerTokens(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BearerTokenOptions>()
                .Bind(configuration.GetSection("BearerTokens"))
                .Validate(bearerTokens =>
                {
                    return bearerTokens.AccessTokenExpirationMinutes < bearerTokens.RefreshTokenExpirationMinutes;
                }, MessageResources.RefreshTokenIsLessThanToken);
        }
        private static void AddApiSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ApiSettings>().Bind(configuration.GetSection("ApiSettings"));
        }
        private static void AddOpenKm(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenKmOptions>(configuration.GetSection(OpenKmOptions.SectionName));
        }
        private static void AddEsbAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            EsbAuthenticationOptions options = configuration
                .GetRequiredSection(EsbAuthenticationOptions.SectionName)
                .Get<EsbAuthenticationOptions>()
                ?? throw new InvalidOperationException("ESB authentication configuration is missing.");

            if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _) ||
                string.IsNullOrWhiteSpace(options.TokenEndpoint) ||
                string.IsNullOrWhiteSpace(options.Username) ||
                string.IsNullOrWhiteSpace(options.Password))
            {
                throw new InvalidOperationException("ESB authentication configuration is invalid.");
            }

            services.AddSingleton(options);
        }
    }
}
