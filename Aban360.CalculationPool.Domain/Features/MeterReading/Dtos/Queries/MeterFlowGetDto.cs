using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterFlowGetDto
    {
        public MeterFlowStepEnum MeterFlowStepId { get; set; }
        public string FileName { get; set; }
        public int ZoneId { get; set; }
        public string InsertDateTime { get; set; }
        public string? Description { get; set; }
    }
}
