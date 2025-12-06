namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterLifeSummaryDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int LessThan5Year { get; set; }
        public int Between5_10Year { get; set; }
        public int Between10_15Year { get; set; }
        public int Between15_20Year { get; set; }
        public int Between20_25Year { get; set; }
        public int Between25_30Year { get; set; }
        public int Between30_35Year { get; set; }
        public int Between35_40Year { get; set; }
        public int Between40_45Year { get; set; }
        public int MoreThan45Year { get; set; }
    }
}
