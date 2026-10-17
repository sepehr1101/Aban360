using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Literals;
using Aban360.Common.Authentication;
using Microsoft.Extensions.Options;

namespace Aban360.BrdigeApi.Extensions
{
    internal static class ConfigureHttpClients
    {
        internal static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEsbTokenProvider, EsbTokenProvider>();
            services.AddTransient<EsbAuthenticationHandler>();
            services.AddOpenKm(configuration);
            services.AddSms();
            return services;
        }
        private static void AddSms(this IServiceCollection services)
        {
            services.AddHttpClient(HttpClientNames.Sms, (sp, httpClient) =>
            {
                SmsOptions options = sp.GetRequiredService<IOptions<SmsOptions>>().Value;
                if (!Uri.TryCreate(options.Url.TrimEnd('/') + "/", UriKind.Absolute, out Uri? baseUri)
                    || (baseUri.Scheme != Uri.UriSchemeHttps && baseUri.Scheme != Uri.UriSchemeHttp))
                    throw new InvalidOperationException(ExceptionLiterals.InvalidConfiguration(nameof(SmsOptions), nameof(SmsOptions.Url)));
                if (string.IsNullOrWhiteSpace(options.Sender))
                    throw new InvalidOperationException(ExceptionLiterals.InvalidConfiguration(nameof(SmsOptions), nameof(SmsOptions.Sender)));
                if (string.IsNullOrWhiteSpace(options.ApiKey))
                    throw new InvalidOperationException(ExceptionLiterals.InvalidConfiguration(nameof(SmsOptions), nameof(SmsOptions.ApiKey)));
                httpClient.BaseAddress = baseUri;
            });
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
            }).AddHttpMessageHandler<EsbAuthenticationHandler>();
        }
    }
}
