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
        private readonly IMemoryCache _catch;
        private const string _accept = "application/json";
        private const string _contentType = "application/json";
        private const string _tokenCacheKey = "CollectBills_AccessToken";
        private const string _bearerToken = "Bearer";
        public CollectBillsService(
            IHttpClientFactory httpClientFactory,
            IOptions<CollectBillsOptions> options,
            IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientNames.CollectBills);
            _httpClient.NotNull(nameof(_httpClient));

            _options = options.Value;
            _options.NotNull(nameof(_options));

            _catch = cache;
            _catch.NotNull(nameof(_catch));
        }

        private async Task<AuthenticationHeaderValue> GetAuthenticationValue()
        {
            CollectBillsLoginOutputDto tokenResult = await GetToken();
            return new AuthenticationHeaderValue(_bearerToken, tokenResult.token_access);
        }
        private async Task<CollectBillsLoginOutputDto> GetToken()
        {
            if (_catch.TryGetValue(_tokenCacheKey, out CollectBillsLoginOutputDto cachedToken))
            {
                return cachedToken;
            }
            CollectBillsLoginOutputDto tokenResult = await GetNewToken();
            var expireSecond = (tokenResult.in_expires - DateTime.Now).TotalSeconds;
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSecond - 30)
            };
            _catch.Set(_tokenCacheKey, tokenResult, cacheOptions);

            return tokenResult;
        }
        private async Task<CollectBillsLoginOutputDto> GetNewToken()
        {
            CollectBillsLoginInputDto loginInput = new(_options.UserName, _options.Password);
            string url = $"{_options.BaseUrl}{_options.Login}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(loginInput), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsLoginOutputDto result = await response.Content.ReadFromJsonAsync<CollectBillsLoginOutputDto>();
            return result;
        }
  
        public async Task<CollectBillsOutputDto<object>> SendCustomerInfo(CollectBillsSubscriptionInfoSendInputDto sampleInputDto)
        {
            string url = $"{_options.BaseUrl}{_options.SubscriptionsInfo}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(sampleInputDto), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationValue();
            //xAuthorization

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

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsUploadOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>> AssignUploadedFile(CollectBillsAssignUploadedFileInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.AssingUploadedFile}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>> GetFileDetails(CollectBillsFileDetailInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.GetFileDetail}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);

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

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsConfirmFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>> GetLastSubscriptionInfoByBillId(string billId)
        {
            string url = $"{_options.BaseUrl}{_options.SubscriptionByBillId}?billId={billId}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsServerConfigOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>>();
            return result;
        }
    }
}
