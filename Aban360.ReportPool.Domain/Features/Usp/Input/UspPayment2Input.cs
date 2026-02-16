namespace Aban360.ReportPool.Domain.Features.Usp.Input
{
    public record UspPayment2Input
    {
        public int VillageOrCityType { get; set; }
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
        public int FromBankCode { get; set; }
        public int ToBankCode { get; set; }
        public short UsageType { get; set; }
        public short GroupingType { get; set; }
        public int ZoneId { get; set; }
        public int ZoneGroupingType { get; set; }
    }
}
