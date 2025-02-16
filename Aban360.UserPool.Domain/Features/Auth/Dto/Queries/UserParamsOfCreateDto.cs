using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserParamsOfCreateDto
    {
        public UserRoleInfo? RoleInfo { get;}
        public LocationTree? LocationTree { get;}
        public AccessTreeValueKeyDto? AccessTree { get;}

        public UserParamsOfCreateDto(UserRoleInfo roleInfo, LocationTree locationTree, AccessTreeValueKeyDto accessTree)
        {
            RoleInfo = roleInfo;
            LocationTree = locationTree;
            AccessTree = accessTree;
        }
    }
}
