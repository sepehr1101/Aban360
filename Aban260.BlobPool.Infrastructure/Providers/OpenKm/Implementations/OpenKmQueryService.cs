using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using HttpClientToCurl;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmQueryService : IOpenKmQueryService
    {
        const string applicationJson = @"application/json";
        private readonly HttpClient _httpClient;
        private readonly OpenKmOptions _options;
        private readonly IMemoryCache _cache;

        private const string TokenCacheKey = "OpenKm_AccessToken";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public OpenKmQueryService(
            IHttpClientFactory httpClientFactory,
            IOptions<OpenKmOptions> options,
            IMemoryCache cache)
        {
            httpClientFactory.NotNull(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(HttpClientNames.Kaj);
            _httpClient.NotNull(nameof(_httpClient));

            _options = options.Value;
            _options.NotNull(nameof(_options));

            _cache = cache;
            _cache.NotNull(nameof(_cache));
        }

        private async Task<TokenResponse> GetToken()
        {
            if (_cache.TryGetValue(TokenCacheKey, out TokenResponse cachedToken))
            {
                return cachedToken;
            }

            var token = await RequestNewToken();

            // set cache with expiration slightly earlier than real expiry
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - 30)
            };

            _cache.Set(TokenCacheKey, token, cacheOptions);

            return token;
        }
        private async Task<TokenResponse> RequestNewToken()
        {
            const string GrantTypeKey = "grant_type";
            const string ClientCredentialsValue = "client_credentials";
            const string BasicScheme = "Basic";
            const string FormUrlEncoded = "application/x-www-form-urlencoded";

            // Prepare form data
            var formData = new Dictionary<string, string>
            {
                { GrantTypeKey, ClientCredentialsValue }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Encode username:password for Basic Auth
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.Username}:{_options.Password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue(BasicScheme, credentials);

            // Explicit content type
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(FormUrlEncoded);

            // Send request
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonOptions);
        }
        private async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()
        {
            TokenResponse token = await GetToken();
            return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        }

        public async Task<FileListResponse> GetFilesByBillId(string billId)
        {
            string fldId = $"{_options.BaseDirectoryPath}{billId}";
            return await GetChildren(fldId);
        }
        private async Task<FileListResponse> GetChildren(string fldId)
        {
            var authHeader = await GetAuthenticationHeaderAsync();

            // Only append the relative path/query; BaseAddress comes from DI
            var requestUrl = $"{_options.GetChildrenEndpoint}?fldId={Uri.EscapeDataString(fldId)}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FileListResponse>(_jsonOptions);
        }

        public async Task<FileListResponse> GetFilesInDirectory(string fieldId)
        {
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build endpoint from config
            string requestUrl = $"{_options.GetFilesListEndpoint}?{Uri.EscapeDataString(fieldId)}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FileListResponse>(_jsonOptions);
        }
        public async Task<byte[]> GetFileBinary(string documentId)
        {
            // Ensure Bearer token
            AuthenticationHeaderValue authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.GetBinaryFileEndpoint}?docId={Uri.EscapeDataString(documentId)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Read as byte array instead of string
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<byte[]> GetImageThumbnail(string documentId)
        {
            // Ensure Bearer token
            AuthenticationHeaderValue authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.GetThumbnailEndpoint}?docId={Uri.EscapeDataString(documentId)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Read as byte array instead of string
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<string> GetDownloadLink(string uuid, bool oneTimeUse)
        {
            var authHeader = await GetAuthenticationHeaderAsync();

            // Only append the relative path/query; BaseAddress comes from DI
            var requestUrl = $"{_options.GetDownloadLinkEndpoint}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = authHeader;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<SearchDocumentsResponse> SearchDocuments(string folderPath, string property, string path)
        {
            // Ensure Bearer token
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build URL from configuration
            string requestUrl = $"{_options.SearchByMetadataEndpoint}?property={Uri.EscapeDataString(property)}&path={Uri.EscapeDataString(path)}";

            // Post folderPath as JSON content
            var content = new StringContent(folderPath, Encoding.UTF8, applicationJson);

            var response = await _httpClient.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SearchDocumentsResponse>(_jsonOptions);
        }
        private void LogRequestToConsole(HttpRequestMessage requestMessage)
        {
            string curl = _httpClient.GenerateCurlInString(
            requestMessage,
            config =>
            {
                config.TurnOn = true;
                config.NeedAddDefaultHeaders = true;
            });
            int x = 2;
        }
    }
}
