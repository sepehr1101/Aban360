using System.Security.Claims;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public class JwtTokenData
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public string RefreshTokenSerial { get; set; } = default!;
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
