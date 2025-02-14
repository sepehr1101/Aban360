using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserDisplayDto
    {
        public UserQueryDto UserInfo { get; set; } = default!;
        public UserRoleInfo? RoleInfo { get; set; }
        public LocationTree? LocationTree { get; set; }
        public AccessTreeValueKeyDto? AccessTree { get; set; }
    }
}
