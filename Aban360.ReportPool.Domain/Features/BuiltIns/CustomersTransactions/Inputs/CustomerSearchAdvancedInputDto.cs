namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerSearchAdvancedInputDto
    {
        public int? CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public int? MeterDiameter { get; set; }
        public string? BillId { get; set; }
        public short? UnitDomesticWater { get; set; }
        public short? UnitCommercialWater { get; set; }
        public short? UnitOtherWater { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public bool? SpecialCustomer { get; set; }
        public bool? CommonSiphon { get; set; }
    }
}
