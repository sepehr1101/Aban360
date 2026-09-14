namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record SmsFlowUpdateDto
    {
        public int Id { get; set; }
        public DateTime SendDateTime { get; set; } = DateTime.Now;
        public SmsFlowUpdateDto(int id)
        {
            Id = id;
        }
        public SmsFlowUpdateDto()
        {
        }
    }
}
