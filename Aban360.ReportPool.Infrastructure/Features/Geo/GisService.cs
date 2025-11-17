using Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Aban360.ReportPool.Infrastructure.Features.Geo
{
    public interface IGisService
    {
        Task<CustomerLocationDto> GetCustomerLocation(CustomerLocationInputDto inputDto);
    }
    internal sealed class GisService : IGisService
    {
        private readonly GeoOptions _options;
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private const string _accept = "*/*";
        private const string _contentType = "application/json";
        public GisService(
            IOptions<GeoOptions> options,
            ITokenService tokenService,
            HttpClient httpClient)
        {
            _options = options.Value;
            _options.NotNull(nameof(_options));

            _tokenService = tokenService;
            _tokenService.NotNull(nameof(tokenService));

            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));
        }

        public async Task<CustomerLocationDto> GetCustomerLocation(CustomerLocationInputDto inputDto)
        {
            var token = await _tokenService.GetToken();
            string requestUrl = _options.BaseUrl + _options.CustomerLocation;

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Authorization = token;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            string billId = JsonSerializer.Serialize(inputDto);
            request.Content = new StringContent(billId, Encoding.UTF8, _contentType);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            CustomerLocationDto result = await response.Content.ReadFromJsonAsync<CustomerLocationDto>();

            return result;
        }
    }
}
