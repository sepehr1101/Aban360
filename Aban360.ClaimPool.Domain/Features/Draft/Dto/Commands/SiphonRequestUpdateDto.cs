using Aban360.ClaimPool.Domain.Features._Base.Dto;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record SiphonRequestUpdateDto : SiphonCommandBaseDto
    {
        public int Id { get; set; }
    }
}
