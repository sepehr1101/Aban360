namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterTagRequestDeleteDto : WaterMeterTagCommandDto
    {
        public int Id { get; set; }
    }
}
