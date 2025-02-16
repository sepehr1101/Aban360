namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record WaterMeterTagGetDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public short WaterMeterTagDefinitionId { get; set; }
        public string? Value { get; set; }
    }
}
