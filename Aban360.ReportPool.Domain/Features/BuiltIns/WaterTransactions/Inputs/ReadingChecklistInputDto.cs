namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ReadingChecklistInputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public int ZoneId { get; set; }
        public bool IsShowLastNumber { get; set; }
    }
}
