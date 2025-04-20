namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record IntervalBillSubscriptionInfoImaginary
    {
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? ReadingNumber { get; set; }
        public string InstallationDate { get; set; } = default!;
        public short ContractualCapacity { get; set; }
        public short HouseholdNumber { get; set; }
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }
        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
        public short EmptyUnit { get; set; }
        public short ConstructionTypeId { get; set; }
        public short UsageConsumptionId { get; set; }
        public short UsageSellId { get; set; }
        public string? SiphonInstallationDate { get; set; }
        public int HeadquarterId { get; set; }
        public int ProvinceId { get; set; }
        public int RegionId { get; set; }
        public int ZoneId { get; set; }
        public int MunicipalityId { get; set; }
        public int PreviousWaterMeterNumber { get; set; }
        public string? PreviousWaterMeterDateJalali { get; set; }
        public Dictionary<short, string>? WaterMeterTags { get; set; }
        public Dictionary<short, string>? IndividualTags { get; set; }
        public Dictionary<short, string>? Discounts { get; set; }
    }
}
