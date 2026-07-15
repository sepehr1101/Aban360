namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterSmsFlowInsertInputDto
    {
        public short FlowId { get; set; }
        public short SmsCount { get; set; }
        public short SmsTemplateId { get; set; }
        public string DueDateJalali { get; set; }
    }
}
