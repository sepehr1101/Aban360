using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterSmsStateTemplateGetDto
    {
        public short Id { get; set; }
        public short SmsTypeId { get; set; }
        public short StepOrder { get; set; }
        public string SmsText { get; set; }
        public short DueDay { get; set; }
        public string? Description { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateTimeJalali { get { return InsertDateTime.ToShortPersianDateTimeString(); } }
        public Guid InsertBy { get; set; }
        public DateTime? RemoveDateTime { get; set; }
        public string? RemovedDateTimeJalali { get { return RemoveDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }
        public Guid? RemoveBy { get; set; }
    }
}
