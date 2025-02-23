namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record ConsumerSummaryDto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public string InstallationDate { get; set; } = default!;
        public string? ProductDate { get; set; }
        public string? GuaranteeDate { get; set; }
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
        public string ConstructionType { get; set; } = default!;
        public string UsageConsumtionTitle { get; set; } = default!;
        public string? UsageSellTitle { get; set; }
        public string FullName { get; set; } = default!;
        public string? SiphonInstallationDate { get; set; }
        public string CordinalDirectionTitle { get; set; } = default!;
        public string HeadquartersTitle { get; set; } = default!;
        public string ProvinceTitle { get; set; } = default!;
        public string RegionTitle { get; set; } = default!;
        public string ZoneTitle { get; set; }=default!;
        public string MunicipalityTitle { get; set; } = default!;
        public ICollection<string>? WaterMeterTags { get; set; }
    }

}