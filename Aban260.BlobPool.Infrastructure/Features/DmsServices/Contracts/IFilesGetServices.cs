using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFilesGetServices
    {
        Task<string> Services(string fieldId);
    }
    internal sealed class FilesGetServices : IFilesGetServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenGetServices _tokenServices;
        string _bearer = "Bearer";
        string _accept = "application/json";
        string _content = "application/json";
        string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-GetFilesList/1.0/";
        public FilesGetServices(IHttpClientFactory httpClientFactory, ITokenGetServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(tokenServices));
        }

        public async Task<string> Services(string fieldId)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_bearer, token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            var values = new
            {
                fldId = fieldId
            };
            // var content=new StringContent(JsonSerializer.Serialize(values),Encoding.UTF8,_content);//?
            string encodedFldId = Uri.EscapeDataString(fieldId);
            string finalUrl = $"{baseUrl}?fldId={encodedFldId}";

            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
