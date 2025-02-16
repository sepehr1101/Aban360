namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record WaterMeterTagCreateDto
    {
        public int WaterMeterId { get; set; }
        public short WaterMeterTagDefinitionId { get; set; }
        public string? Value { get; set; }
    }
}
