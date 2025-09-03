using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Implementations
{
    internal sealed class OpenKmQueryService
    {
        const string applicationJson = @"application/json";
        private readonly HttpClient _httpClient;
        private readonly OpenKmOptions _options; 
        private readonly IMemoryCache _cache;

        private const string TokenCacheKey = "OpenKm_AccessToken";

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
            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        private async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()
        {
            TokenResponse token = await GetToken();
            return new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        }

        public async Task<FileListResponse> GetFilesByBillId(string billId)
        {
            string fldId = $"{_options.BaseDirectoryPath}/{billId}";
            return await GetChildren(fldId);
        }
        private async Task<FileListResponse> GetChildren(string fldId)
        {
            // Ensure Bearer token
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Set headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            // Build request URL from configuration
            string requestUrl = $"{_options.GetChildrenEndpoint}?fldId={Uri.EscapeDataString(fldId)}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize into strongly typed object
            return JsonSerializer.Deserialize<FileListResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<FileListResponse> GetFilesInDirectory(string fieldId)
        {
            var authHeader = await GetAuthenticationHeaderAsync();
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;

            // Build endpoint from config
            string requestUrl = $"{_options.GetFilesListEndpoint}?{Uri.EscapeDataString(fieldId)}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<FileListResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
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
        public async Task<string> GetDownloadLink(string uuid, bool oneTimeUse)
        {
            string _BaseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetDownloadLinkFile/1.0";
            string _accept = "application/xml";
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            //client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            string url = $"{_BaseUrl}?uuid={uuid}&oneTimeUse={oneTimeUse.ToString().ToLower()}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
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

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize into strongly typed model
            return JsonSerializer.Deserialize<SearchDocumentsResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

    }
}
