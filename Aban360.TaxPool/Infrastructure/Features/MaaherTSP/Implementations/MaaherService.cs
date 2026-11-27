using Aban360.Common.Extensions;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Implementations
{
    internal sealed class MaaherService : IMaaherService
    {
        private readonly HttpClient _httpClient;
        private const string _applicationJson = "application/json";
        private readonly MaaherOptions _options;
        public MaaherService(
            HttpClient httpClient,
           IOptions<MaaherOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));

            _options = options.Value;
            _options.NotNull(nameof(options));
        }

        public async Task<ICollection<MaaherResponseNew>> SendInvoice(ICollection<MaaherRequestWrapper_New> inputDto)
        {
            string sendUrl = string.Format(_options.SentInvoiceUrl, _options.Tins, _options.FiscalId, _options.AuthenticationId);
            string url = _options.MaaherBaseUrl + sendUrl;
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_applicationJson));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.Token);

            var body = JsonSerializer.Serialize(inputDto);
            var content = new StringContent(body, Encoding.UTF8, _applicationJson);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ICollection<MaaherResponseNew> invoicesRecieve = JsonSerializer.Deserialize<List<MaaherResponseNew>>(result, options);
            return invoicesRecieve;

        }
    }
}
