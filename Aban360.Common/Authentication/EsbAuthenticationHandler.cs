using System.Net;

namespace Aban360.Common.Authentication
{
    public sealed class EsbAuthenticationHandler : DelegatingHandler
    {
        private readonly IEsbTokenProvider _tokenProvider;

        public EsbAuthenticationHandler(IEsbTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authentication = await _tokenProvider.GetAuthenticationHeaderAsync(cancellationToken);
            request.Headers.Authorization = authentication;

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            using HttpRequestMessage retryRequest = await CloneAsync(request, cancellationToken);
            response.Dispose();

            await _tokenProvider.InvalidateAsync(authentication.Parameter!, cancellationToken);
            retryRequest.Headers.Authorization = await _tokenProvider.GetAuthenticationHeaderAsync(cancellationToken);
            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private static async Task<HttpRequestMessage> CloneAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
                VersionPolicy = request.VersionPolicy
            };

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var option in request.Options)
            {
                clone.Options.TryAdd(option.Key, option.Value);
            }

            if (request.Content is not null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                clone.Content = new ByteArrayContent(content);
                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }
    }
}
