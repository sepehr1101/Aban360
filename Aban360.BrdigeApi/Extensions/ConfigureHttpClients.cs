using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Literals;
using Microsoft.Extensions.Options;

namespace Aban360.BrdigeApi.Extensions
{
    internal static class ConfigureHttpClients
    {
        internal static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenKm(configuration);
            return services;
        }
        private static void AddOpenKm(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(HttpClientNames.Kaj, (sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<OpenKmOptions>>();
                if (options is null || string.IsNullOrWhiteSpace(options.Value.BaseUrl))
                {
                    throw new InvalidOperationException(ExceptionLiterals.InvalidConfiguration(nameof(OpenKmOptions), nameof(OpenKmOptions.BaseUrl)));
                }
                httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            });
        }
    }
}
