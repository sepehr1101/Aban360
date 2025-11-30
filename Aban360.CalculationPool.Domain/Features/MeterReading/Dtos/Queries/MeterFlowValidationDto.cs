using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterFlowValidationDto
    {
        public int Id { get; set; }
        public string? RemovedDateTime { get; set; }
        public string InsertDateTime { get; set; }
        public MeterFlowStepEnum MeterFlowStepId{ get; set; }
    }
}
