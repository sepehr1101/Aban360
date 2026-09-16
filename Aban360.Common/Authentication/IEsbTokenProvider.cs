using System.Net.Http.Headers;

namespace Aban360.Common.Authentication
{
    public interface IEsbTokenProvider
    {
        Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken = default);
        Task InvalidateAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}
