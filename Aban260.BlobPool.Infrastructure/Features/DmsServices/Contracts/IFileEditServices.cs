using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface IFileEditServices
    {
        Task Services(string nodeId, string groupName);
    }
    internal sealed class FileEditServices : IFileEditServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenGetServices _tokenServices;
        string bearer = "Bearer";
        string accept="application/xml";
        string cookie = "Cookie";
        string cookieDate = "cookiesession1=678ADA5C33A30F49D180AB6CBD34D5FC";
        string baseUrl = $"https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-CreateMetadata/1.0";     
        public FileEditServices(IHttpClientFactory httpClientFactory, ITokenGetServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(_httpClientFactory));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(_tokenServices));
        }

        public async Task Services(string nodeId,string groupName)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(bearer, token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            client.DefaultRequestHeaders.Add(cookie, cookieDate);

            string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

            var response=await client.PostAsync(finalUrl, null);
            response.EnsureSuccessStatusCode();
            var result=await response.Content.ReadAsStringAsync();
        }
    }
}
