using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterFlowCartableGetDto
    {
        public short Id { get; set; }
        public MeterFlowStepEnum MeterFlowStepId { get; set; }
        public string StepTitle { get; set; }
        public string FileName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid InsertByUserId { get; set; }
        public string? Description { get; set; }
       
    }
}
