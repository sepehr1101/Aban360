using Aban360.UserPool.Persistence.Constants.Enums;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserSearchByRoleTitleAndZoneIdsDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public ClaimType ClaimType { get; set; }
        public string RoleName { get; set; }
        public UserSearchByRoleTitleAndZoneIdsDto(IEnumerable<int> zoneIds, ClaimType claimType, string roleName)
        {
            ZoneIds = zoneIds;
            ClaimType = claimType;
            RoleName = roleName;
        }
        public UserSearchByRoleTitleAndZoneIdsDto()
        {
        }
    }
}
