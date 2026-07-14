namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterSmsFlowGetDto
    {
        public int Id { get; set; }
        public short FlowId { get; set; }
        public short SmsCount { get; set; }
        public short SmsTemplateId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime DueDateTime { get; set; }
        public DateTime? SendDateTime { get; set; }
    }
}
