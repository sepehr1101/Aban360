namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ConsumerSummaryDto
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public string RequestDate { get; set; } = default!;
        public string InstallationDate { get; set; } = default!;
        public string? ProductDate { get; set; }
        public string? GuaranteeDate { get; set; }
        public string Address { get; set; } = default!;
        public string? CounterState { get; set; }
        public string? CounterStatus { get; set; }
        public short ContractualCapacity { get; set; }
        public short HouseholdNumber { get; set; }
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }
        public int TotalUnitWater { get; set; }

        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
        public int TotalUnitSewage { get; set; }

        public short EmptyUnit { get; set; }
        public string ConstructionType { get; set; } = default!;
        public string UsageConsumption { get; set; } = default!;
        public string UsageSell { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string SiphonRequestDate { get; set; } = default!;
        public string? SiphonInstallationDate { get; set; }
        public string? HeadquartersTitle { get; set; }
        public string? CordinalDirectionTitle { get; set; }
        public string? ProvinceTitle { get; set; }
        public string? RegionTitle { get; set; }
        public string? ZoneTitle { get; set; }
        public string? MunicipalityTitle { get; set; }
        public bool HasSewage { get; set; } = default!;
        public ICollection<string>? WaterMeterTags { get; set; }


        public string DeletionStateTitle { get; set; }
        public int MunicipalityId { get; set; }
        public int ZoneId { get; set; }
        public string? PostalCode{ get; set; }
        public string? MobileNumber { get; set; }
        public string DiscountType { get; set; } = default!;
        public string MeterDiameterTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; } = default!;
    }
}