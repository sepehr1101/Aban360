namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterTagRequestUpdateDto : WaterMeterTagCommandDto
    {
        public int Id { get; set; }
    }
}
