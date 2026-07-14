namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterSmsStateTemplateRemoveDto
    {
        public short Id { get; set; }
        public DateTime RemoveDateTime { get; set; }
        public Guid RemoveBy { get; set; }
        public MeterSmsStateTemplateRemoveDto(short id, Guid removeBy)
        {
            Id = id;
            RemoveDateTime = DateTime.Now;
            RemoveBy = removeBy;
        }
        public MeterSmsStateTemplateRemoveDto()
        {
        }
    }
}

