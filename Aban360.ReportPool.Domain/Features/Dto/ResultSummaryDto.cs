namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record ResultSummaryDto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string? ReadingNumber { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? ProductDate { get; set; }
        public DateTime? GuaranteeDate { get; set; }
        public string Address { get; set; }
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
        public DateTime? SiphonInstallationDate { get; set; }
        public string Headquarter { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Municipality { get; set; }
        public ICollection<string>? WaterMeterTags { get; set; }
    }

}