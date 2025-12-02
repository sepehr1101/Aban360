using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingCheckedOutputDto
    {
        public int Id { get; set; }
        public MeterFlowStepEnum MeterFlowStepId { get; set; }
        public string Message { get; set; }
        public MeterReadingCheckedOutputDto(int id, MeterFlowStepEnum meterFlowStepId, string message)
        {
            Id = id;
            MeterFlowStepId = meterFlowStepId;
            Message = message;  
        }
    }
}
