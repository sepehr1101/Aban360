namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record SmsStateTemplateInsertDto
    {
        public short GroupId { get; set; }
        public short SmsTypeId { get; set; }
        public short StepOrder { get; set; }
        public string SmsText { get; set; }
        public short DueDay { get; set; }
        public string? Description { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public Guid InsertBy { get; set; }
    }
}

