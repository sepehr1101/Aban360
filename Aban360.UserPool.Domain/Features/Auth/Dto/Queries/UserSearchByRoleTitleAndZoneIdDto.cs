using Aban360.UserPool.Persistence.Constants.Enums;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserSearchByRoleTitleAndZoneIdDto
    {
        public int ZoneId { get; set; }
        public ClaimType ClaimType { get; set; }
        public string RoleName { get; set; }
        public UserSearchByRoleTitleAndZoneIdDto(int zoneId, ClaimType claimType, string roleName)
        {
            ZoneId = zoneId;
            ClaimType = claimType;
            RoleName = roleName;
        }
        public UserSearchByRoleTitleAndZoneIdDto()
        {
        }
    }
}
