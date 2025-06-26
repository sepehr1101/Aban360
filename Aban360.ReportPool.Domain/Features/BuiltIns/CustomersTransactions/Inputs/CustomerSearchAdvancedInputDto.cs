namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerSearchAdvancedInputDto
    {
        public int? CustomerNumber { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public int? MeterDiameter { get; set; }
        public string? BillId { get; set; }
        public short? FromUnitDomesticWater { get; set; }
        public short? ToUnitDomesticWater { get; set; }
        public short? FromUnitCommercialWater { get; set; }
        public short? ToUnitCommercialWater { get; set; }
        public short? FromUnitOtherWater { get; set; }
        public short? ToUnitOtherWater { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public bool? SpecialCustomer { get; set; }
        public bool? CommonSiphon { get; set; }
        public ICollection<int>? ZoneIds { get; set; }
        public int? FromContractualCapacity { get; set; }
        public int? ToContractualCapacity { get; set; }
        public int? FromHousholderNumber { get; set; }
        public int? ToHousholderNumber { get; set; }
        public ICollection<int>? UsageIds { get; set; }
    }
}
