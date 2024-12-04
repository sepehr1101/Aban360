using Aban360.UserPool.Persistence.Constants.Enums;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Base
{
    public class ClaimDto
    {
        public ClaimType ClaimTypeId { get; init; }
        public string ClaimValue { get; init; } = null!;
    }
}
