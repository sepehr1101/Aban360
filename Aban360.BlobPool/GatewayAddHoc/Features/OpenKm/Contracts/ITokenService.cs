using System.Net.Http.Headers;

namespace Aban360.BlobPool.GatewayAddHoc.Features.OpenKm.Contracts
{
    public interface ITokenService
    {
        Task<AuthenticationHeaderValue> GetToken();
    }
}
