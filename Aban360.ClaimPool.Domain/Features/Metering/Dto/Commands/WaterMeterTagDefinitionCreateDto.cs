namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record WaterMeterTagDefinitionCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
    }
}
