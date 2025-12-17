namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ClosedInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public bool IsZoneGrouped { get; set; }

    }
}
