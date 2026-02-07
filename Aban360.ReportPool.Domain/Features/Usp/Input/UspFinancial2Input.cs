namespace Aban360.ReportPool.Domain.Features.Usp.Input
{
    public record UspFinancial2Input
    {
        public int VillageOrCityType { get; set; }
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
        public short UsageType { get; set; }
        public short GroupingType { get; set; }
        public short NetType { get; set; }
        public short BranchGroupType { get; set; }
        public int ZoneId { get; set; }
        public int ZoneGroupingType { get; set; }
    }
}
