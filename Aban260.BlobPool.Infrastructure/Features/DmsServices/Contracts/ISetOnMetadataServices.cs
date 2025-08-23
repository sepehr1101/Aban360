using Aban360.Common.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts
{
    public interface ISetOnMetadataServices
    {
        Task Service(string body, string nodeId, string groupName);
    }
    internal sealed class SetOnMetadataServices : ISetOnMetadataServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string accept = "application/xml";
        string textPlain = "text/plain";
        string baseUrl = "https://esb.abfaisfahan.com:8243/DMS-Moshtarakin-SetMetadata/1.0";
        public SetOnMetadataServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
        }

        public async Task Service(string body, string nodeId, string groupName)
        {
            var client = _httpClientFactory.CreateClient("token");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

            var content = new StringContent(body, Encoding.UTF8, textPlain);
            string finalUrl = $"{baseUrl}?nodeId={nodeId}&grpName={groupName}";

            var response = await client.PutAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
