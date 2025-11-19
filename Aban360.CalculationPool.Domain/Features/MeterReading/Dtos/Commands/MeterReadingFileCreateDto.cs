namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileCreateDto
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public int RecordCount { get; set; }
        public short AgentCode { get; set; }
        public int ZoneId { get; set; }
        public Guid InsertByUserId { get; set; }
        public DateTime InsertDateTime { get; set; }

    }
}
