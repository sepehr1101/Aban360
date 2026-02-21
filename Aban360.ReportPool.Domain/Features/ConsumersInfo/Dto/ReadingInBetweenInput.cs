namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ReadingInBetweenInput
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; } = default!;
        public string ToReadingNumber { get; set; } = default!;
    }
}
