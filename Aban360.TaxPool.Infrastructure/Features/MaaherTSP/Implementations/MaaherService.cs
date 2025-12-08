using Aban360.Common.Extensions;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Implementations
{
    internal sealed class MaaherService : IMaaherService
    {
        private readonly HttpClient _httpClient;
        private const string _applicationJson = "application/json";
        public MaaherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));
        }

        public async Task<ICollection<SentInvoiceRecieveDto>> SendInvoice(ICollection<MaaherTSPInvoiceDto> inputDto)
        {
            //todo: get token
            string url = MaaherTSPUri.SandboxUrl + MaaherTSPUri.SentInvoiceUrl("1", "1", "");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_applicationJson));

            var body = JsonSerializer.Serialize(inputDto);
            var content = new StringContent(body, Encoding.UTF8, _applicationJson);

            var response = await _httpClient.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ICollection<SentInvoiceRecieveDto> invoicesRecieve = JsonSerializer.Deserialize<List<SentInvoiceRecieveDto>>(result,options);
            return invoicesRecieve;

        }
    }
}
