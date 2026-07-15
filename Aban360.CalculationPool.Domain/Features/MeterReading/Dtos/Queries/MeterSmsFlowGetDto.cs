using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterSmsFlowGetDto
    {
        public int Id { get; set; }
        public short FlowId { get; set; }
        public short SmsCount { get; set; }
        public short SmsTemplateId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateTimeJalali { get { return InsertDateTime.ToShortPersianDateTimeString(); } }
        public Guid InsertBy { get; set; }
        public DateTime DueDateTime { get; set; }
        public string? DueDateTimeJalali { get { return DueDateTime.ToShortPersianDateTimeString(); } }
        public DateTime? SendDateTime { get; set; }
        public string? SendDateTimeJalali { get { return SendDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }

    }
}
