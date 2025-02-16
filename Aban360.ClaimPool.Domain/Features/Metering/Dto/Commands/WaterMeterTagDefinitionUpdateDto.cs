namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record WaterMeterTagDefinitionUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
    }
}
