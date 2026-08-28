using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Contracts;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Implementations
{
    internal sealed class CollectBillsService : ICollectBillsService
    {
        private readonly HttpClient _httpClient;
        private readonly CollectBillsOptions _options;
        private readonly IMemoryCache _cache;
        private const string _accept = "application/json";
        private const string _contentType = "application/json";
        const string _formUrlEncodedContentType = "application/x-www-form-urlencoded";
        private const string _tokenCacheKey = "CollectBills_AccessToken";
        public CollectBillsService(
            IHttpClientFactory httpClientFactory,
            IOptions<CollectBillsOptions> options,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.CollectBills);
            _httpClient.NotNull(nameof(_httpClient));

            _options = options.Value;
            _options.NotNull(nameof(_options));

            _cache = cache;
            _cache.NotNull(nameof(_cache));
        }
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private async Task<TokenResponse> GetToken()
        {
            if (_cache.TryGetValue(_tokenCacheKey, out TokenResponse cachedToken))
            {
                return cachedToken;
            }

            var token = await RequestNewToken();

            // set cache with expiration slightly earlier than real expiry
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - 30)
            };

            _cache.Set(_tokenCacheKey, token, cacheOptions);

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
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.UserName}:{_options.Password}"));
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
        public async Task<CollectBillsOutputDto<object>> SendCustomerInfo(CollectBillsSubscriptionInfoSendInputDto sampleInputDto)
        {
            string url = $"{_options.BaseUrl}{_options.SubscriptionsInfo}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(sampleInputDto), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<object> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<object>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsUploadOutputDto>> Upload(CollectBillsUploadInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.Upload}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();

            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsUploadOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>> AssignUploadedFile(CollectBillsAssignUploadedFileInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.AssingUploadedFile}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>> GetFileDetails(CollectBillsFileDetailInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.GetFileDetail}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsFileDetailOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>> ConfirmFileBills(CollectBillsConfirmFileInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.ConfirmFile}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsConfirmFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>> GetLastSubscriptionInfoByBillId(string billId)
        {
            string url = $"{_options.BaseUrl}{_options.SubscriptionByBillId}?billId={billId}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Headers.Authorization = await GetAuthenticationHeaderAsync();

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsServerConfigOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>>();
            return result;
        }
    }
}
