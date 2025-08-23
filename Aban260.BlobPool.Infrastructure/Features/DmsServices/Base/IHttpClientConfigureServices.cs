using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices.Base
{
    public interface IHttpClientConfigureServices
    {
        void Services(HttpClient client, HttpClientParameters parameters);
    }
    internal sealed class HttpClientConfigureServices : IHttpClientConfigureServices
    {
        string baseUrl = $"https://esb.abfaisfahan.com:8243/";

        public void Services(HttpClient client, HttpClientParameters parameters)
        {
            client.BaseAddress=new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(parameters.AuthenticationType, parameters.AuthenticationValue);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(parameters.Accept));
        }
    }
    public record HttpClientParameters
    {
        public string? Accept { get; set; }
        public string? Content { get; set; }
        public string? AuthenticationType { get; set; }
        public string? AuthenticationValue { get; set; }
        public Dictionary<string, string>? BodyData { get; set; }
        public HttpClientParameters(string token)
        {
            AuthenticationValue = token;
        }
    }
}
