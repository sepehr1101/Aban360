namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterSmsFlowInsertDto
    {
        public short FlowId { get; set; }
        public short SmsCount { get; set; }
        public short SmsTemplateId { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public Guid InsertBy { get; set; }
        public DateTime DueDateTime { get; set; }
    }
}
