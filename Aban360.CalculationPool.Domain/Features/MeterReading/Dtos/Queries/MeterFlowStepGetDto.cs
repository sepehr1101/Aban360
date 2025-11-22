namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterFlowStepGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
