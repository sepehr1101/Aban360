using Aban360.ClaimPool.Domain.Features._Base.Dto;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record EstateRequestUpdateDto : EstateCommandBaseDto
    {
        public int Id { get; set; }
    }
}