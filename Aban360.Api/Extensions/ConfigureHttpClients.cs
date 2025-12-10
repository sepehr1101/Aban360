using Aban360.Api.Exceptions;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto;
using Microsoft.Extensions.Options;

namespace Aban360.Api.Extensions
{
    internal static class ConfigureHttpClients
    {       
        internal static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenKm(configuration);
            services.AddGeo(configuration);
            services.AddMaaher(configuration);
            return services;
        }
        private static void AddOpenKm(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(HttpClientNames.Kaj, (sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<OpenKmOptions>>();
                if (options is null || string.IsNullOrWhiteSpace(options.Value.BaseUrl))
                {
                    throw new InvalidConfigFileException(ExceptionLiterals.InvalidConfiguration(nameof(OpenKmOptions), nameof(OpenKmOptions.BaseUrl)));
                }
                httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            });
        }
        private static void AddGeo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(HttpClientNames.Geo, (sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GeoOptions>>();
                if (options is null || string.IsNullOrWhiteSpace(options.Value.BaseUrl))
                {
                    throw new InvalidConfigFileException(ExceptionLiterals.InvalidConfiguration(nameof(GeoOptions), nameof(GeoOptions.BaseUrl)));
                }
                httpClient.BaseAddress=new Uri(options.Value.BaseUrl);
            });
        }
        private static void AddMaaher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(HttpClientNames.Maaher, (sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<MaaherOptions>>();
                if (options is null || string.IsNullOrWhiteSpace(options.Value.MaaherBaseUrl))
                {
                    throw new InvalidConfigFileException(ExceptionLiterals.InvalidConfiguration(nameof(MaaherOptions), nameof(MaaherOptions.MaaherBaseUrl)));
                }
                httpClient.BaseAddress = new Uri(options.Value.MaaherBaseUrl);
            });
        }
    }
}
