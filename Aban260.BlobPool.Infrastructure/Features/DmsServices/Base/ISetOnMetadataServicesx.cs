using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Base
{
    public interface ISetOnMetadataServicesx
    {
        Task Service(string body, string nodeId, string groupName);
    }
    internal sealed class SetOnMetadataServicesx : ISetOnMetadataServicesx
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientConfigureServices _configureServices;
        private readonly ITokenGetServices _tokenServices;

        string bearer = "Bearer";
        string accept = "application/xml";
        string textPlain = "text/plain";
        string Url = "DMS-Moshtarakin-SetMetadata/1.0";

        public SetOnMetadataServicesx(
            IHttpClientFactory httpClientFactory,
            IHttpClientConfigureServices configureServices,
            ITokenGetServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));

            _configureServices = configureServices;
            _configureServices.NotNull(nameof(configureServices));

            _tokenServices = tokenServices;
            _tokenServices.NotNull(nameof(tokenServices));
        }

        public async Task Service(string body, string nodeId, string groupName)
        {
            var client = _httpClientFactory.CreateClient();
            string token = await _tokenServices.Service();
            HttpClientParameters clientParams = new(token)
            {
                AuthenticationType = bearer,
                Accept=accept,
            };
            _configureServices.Services(client, clientParams);

            var content = new StringContent(body, Encoding.UTF8, textPlain);
            string finalUrl = $"{Url}?nodeId={nodeId}&grpName={groupName}";

            var response = await client.PutAsync(finalUrl, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}