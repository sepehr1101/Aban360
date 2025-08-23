using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban260.BlobPool.Infrastructure.Features.DmsServices
{
    public class TokenInterceptor : DelegatingHandler
    {
        private readonly ITokenGetServices _tokenGetServices;
        string _bearer = "Bearer";
        public TokenInterceptor(ITokenGetServices tokenGetServices)
        {
            _tokenGetServices = tokenGetServices;
            _tokenGetServices.NotNull(nameof(tokenGetServices));
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await _tokenGetServices.Service();

            request.Headers.Authorization =new AuthenticationHeaderValue(_bearer,token); 

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
