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
            //var expireSecond = (tokenResult.Parameters.ExpirationDateTime - DateTime.Now).TotalSeconds;//todo: expireDate?
            var expireSecond = 3600;
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
        private async Task<CollectBillsInputDto<T>> GetInputDto<T>(T inputDto)
        {
            CollectBillsLoginOutputDto token = await GetToken();
            CollectBillsIdentityInputDto identity = new(token.token_access);
            return new CollectBillsInputDto<T>(inputDto, identity);
        }

        //*
        public async Task<CollectBillsOutputDto<CollectBillsUploadOutputDto>> SendCustomerInfo(IEnumerable<string> sampleInputDto)
        {
            string url = $"{_options.BaseUrl}{_options.Upload}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(sampleInputDto), Encoding.UTF8, _contentType);
            request.Headers.Authorization = await GetAuthenticationValue();
            //xAuthorization

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsUploadOutputDto>>();
            return result;
        }


        public async Task<CollectBillsOutputDto<CollectBillsUploadOutputDto>> Upload(CollectBillsUploadInputDto input)
        {
            CollectBillsInputDto<CollectBillsUploadInputDto> inputDto = await GetInputDto(input);

            string url = $"{_options.BaseUrl}{_options.Upload}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(inputDto), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsUploadOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>> AssignUploadedFile(CollectBillsAssignUploadedFileInputDto input)
        {
            CollectBillsInputDto<CollectBillsAssignUploadedFileInputDto> inputDto = await GetInputDto(input);

            string url = $"{_options.BaseUrl}{_options.AssingUploadedFile}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(inputDto), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>> GetFileDetails(CollectBillsFileDetailInputDto input)
        {
            CollectBillsInputDto<CollectBillsFileDetailInputDto> inputDto = await GetInputDto(input);

            string url = $"{_options.BaseUrl}{_options.GetFileDetail}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(inputDto), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsFileDetailOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsFileDetailOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>> GetServiceConfigForPanel(CollectBillsIdentityInputDto input)
        {
            string url = $"{_options.BaseUrl}{_options.GetServiceConfigure}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsServerConfigOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsServerConfigOutputDto>>();
            return result;
        }
        public async Task<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>> ConfirmFileBills(CollectBillsConfirmFileInputDto input)
        {
            CollectBillsInputDto<CollectBillsConfirmFileInputDto> inputDto = await GetInputDto(input);


            string url = $"{_options.BaseUrl}{_options.ConfirmFile}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            request.Content = new StringContent(JsonSerializer.Serialize(inputDto), Encoding.UTF8, _contentType);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            CollectBillsOutputDto<CollectBillsConfirmFileOutputDto> result = await response.Content.ReadFromJsonAsync<CollectBillsOutputDto<CollectBillsConfirmFileOutputDto>>();
            return result;
        }
    }
}
