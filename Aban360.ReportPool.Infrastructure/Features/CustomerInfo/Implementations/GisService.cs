using Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.CustomerInfo.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Aban360.ReportPool.Infrastructure.Features.CustomerInfo.Implementations
{
    internal sealed class GisService : IGisService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private const string _url = @"http://esb.abfaisfahan.com:8280/GIS-CustomerLocation/1.0";
        private const string _accept = "*/*";
        private const string _contentType = "application/json";
        public GisService(
            ITokenService tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
            _tokenService.NotNull(nameof(tokenService));

            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));
        }

        public async Task<CustomerLocationDto> GetCustomerLocation(CustomerLocationInputDto inputDto)
        {
            var token = await _tokenService.GetToken();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = token;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            string billId = JsonSerializer.Serialize(inputDto);
            var content = new StringContent(billId, Encoding.UTF8, _contentType);

            var response = await _httpClient.PutAsync(_url, content);
            response.EnsureSuccessStatusCode();
            CustomerLocationDto result = await response.Content.ReadFromJsonAsync<CustomerLocationDto>();
            return result;
        }
    }
}
