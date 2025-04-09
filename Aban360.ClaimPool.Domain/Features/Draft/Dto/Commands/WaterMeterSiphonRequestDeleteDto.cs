namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterSiphonRequestDeleteDto : WaterMeterSiphonCommandDto
    {
        public int Id { get; set; }
    }
}
