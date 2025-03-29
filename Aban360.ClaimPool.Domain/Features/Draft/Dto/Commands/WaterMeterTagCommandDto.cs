namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterTagCommandDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public short WaterMeterTagDefinitionId { get; set; }
        public string? Value { get; set; }

    }
}
