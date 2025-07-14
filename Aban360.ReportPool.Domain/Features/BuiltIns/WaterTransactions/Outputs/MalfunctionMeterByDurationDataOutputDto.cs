namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterByDurationDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string MyProperty { get; set; }
    }
}
