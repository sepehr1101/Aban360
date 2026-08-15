namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ReadingSequenceInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int ZoneId { get; set; }
    }
}
