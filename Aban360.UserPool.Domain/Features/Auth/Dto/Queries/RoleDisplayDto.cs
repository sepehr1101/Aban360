using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record RoleDisplayDto
    {
        public RoleGetDto RoleInfo { get; set; } = default!;
        public AccessTreeValueKeyDto AccessTree { get; set; }= default!;
        public RoleDisplayDto(RoleGetDto roleGetDto, AccessTreeValueKeyDto accessTree)
        {
            RoleInfo = roleGetDto;
            AccessTree = accessTree;
        }
        public RoleDisplayDto()
        {
            
        }
    }
}
