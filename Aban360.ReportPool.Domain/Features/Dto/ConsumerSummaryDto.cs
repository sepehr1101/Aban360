namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record ConsumerSummaryDto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public string InstallationDate { get; set; }
        public string ProductDate { get; set; }
        public string GuaranteeDate { get; set; }
        public string Address { get; set; } = default!;
        public short ContractualCapacity { get; set; }
        public short HouseholdNumber { get; set; }
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }
        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
        public short EmptyUnit { get; set; }
        public string ConstructionType { get; set; }
        public string UsageConsumtionTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public string FullName { get; set; }
        public string SiphonInstallationDate { get; set; }
        public string Headquarter { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Municipality { get; set; }
        public ICollection<string>? WaterMeterTags { get; set; }
    }

}