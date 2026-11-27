using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Contracts;
using Aban360.Common.Extensions;
using Aban360.Common.Authentication;
using System.Net.Http.Headers;

namespace Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Implementations
{
    internal sealed class TokenService : ITokenService
    {
        private readonly IEsbTokenProvider _tokenProvider;
        public TokenService(IEsbTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _tokenProvider.NotNull(nameof(tokenProvider));
        }

        public async Task<AuthenticationHeaderValue> GetToken()
        {
            return await _tokenProvider.GetAuthenticationHeaderAsync();
        }
    }
}
