using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Contracts;
using Aban360.Common.Extensions;
using System.Net.Http.Headers;

namespace Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Implementations
{
    internal sealed class TokenService : ITokenService
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public TokenService(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<AuthenticationHeaderValue> GetToken()
        {
            return await _openKmQueryService.GetAuthenticationHeaderAsync();
        }
    }
}
