namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterSmsFlowUpdateDto
    {
        public int Id { get; set; }
        public DateTime SendDateTime { get; set; }
    }
}
