using Aban360.ClaimPool.Domain.Features._Base.Dto;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterRequestUpdateDto : WaterMeterCommandBaseDto
    {
        public int Id { get; set; }
    }
}
