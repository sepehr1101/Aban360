namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record SmsFlowInsertDto
    {
        public int FirstFlowId { get; set; }
        public int SmsCount { get; set; }
        public short SmsTemplateId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime DueDateTime { get; set; }
    }
}
