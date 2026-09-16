using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Aban360.Common.Authentication
{
    public sealed class EsbTokenProvider : IEsbTokenProvider, IDisposable
    {
        private static readonly TimeSpan ExpirationSafetyWindow = TimeSpan.FromSeconds(30);

        private readonly HttpClient _httpClient;
        private readonly EsbAuthenticationOptions _options;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private CacheEntry? _cacheEntry;

        public EsbTokenProvider(EsbAuthenticationOptions options)
        {
            _options = options;
            _httpClient = new HttpClient { BaseAddress = new Uri(_options.BaseUrl) };
        }

        public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken = default)
        {
            EsbToken token = await GetTokenAsync(cancellationToken);
            return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        }

        public async Task InvalidateAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                if (_cacheEntry?.Token.AccessToken == accessToken)
                {
                    _cacheEntry = null;
                }
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<EsbToken> GetTokenAsync(CancellationToken cancellationToken)
        {
            if (HasValidToken())
            {
                return _cacheEntry!.Token;
            }

            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                if (HasValidToken())
                {
                    return _cacheEntry!.Token;
                }

                EsbToken token = await RequestNewTokenAsync(cancellationToken);
                TimeSpan cacheLifetime = TimeSpan.FromSeconds(token.ExpiresIn) - ExpirationSafetyWindow;
                if (cacheLifetime <= TimeSpan.Zero)
                {
                    cacheLifetime = TimeSpan.FromSeconds(1);
                }

                _cacheEntry = new CacheEntry(token, DateTimeOffset.UtcNow.Add(cacheLifetime));
                return token;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private bool HasValidToken() =>
            _cacheEntry is not null && DateTimeOffset.UtcNow < _cacheEntry.RefreshAtUtc;

        private async Task<EsbToken> RequestNewTokenAsync(CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials"
                })
            };

            string credentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_options.Username}:{_options.Password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            EsbToken? token = await response.Content.ReadFromJsonAsync<EsbToken>(cancellationToken: cancellationToken);
            if (token is null || string.IsNullOrWhiteSpace(token.AccessToken) || string.IsNullOrWhiteSpace(token.TokenType))
            {
                throw new InvalidOperationException("ESB returned an invalid token response.");
            }

            return token;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _refreshLock.Dispose();
        }

        private sealed record CacheEntry(EsbToken Token, DateTimeOffset RefreshAtUtc);
    }
}
