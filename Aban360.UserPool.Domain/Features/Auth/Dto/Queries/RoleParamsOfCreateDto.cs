using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record RoleParamsOfCreateDto
    {
        public AccessTreeValueKeyDto AccessTreeValueKeyDto { get;} = default!;
        public RoleParamsOfCreateDto(AccessTreeValueKeyDto accessTreeValueKeyDto)
        {
            AccessTreeValueKeyDto = accessTreeValueKeyDto;
        }
    }
}
