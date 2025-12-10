using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto;
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
            services.AddGeo(configuration);
            services.AddMaaher(configuration);
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
        private static void AddGeo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GeoOptions>(configuration.GetSection(GeoOptions.SectionName));
        }
        private static void AddMaaher(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MaaherOptions>(configuration.GetSection(MaaherOptions.SectionName));
        }
    }
}
