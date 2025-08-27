namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionToChangeSummaryDataOutputFromDataBaseDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string ChangeDateJalali { get; set; }
        public string LatestMalfunctionDateJalali { get; set; }
        public string Duration { get; set; }

    }
}
