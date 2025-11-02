namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerSearchDataOutputDto
    {
        public string? ZoneTitle { get; set; }
        public int? CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NationalCode { get; set; }
        public string? PostalCode { get; set; }
        public string? MeterDiameterTitle { get; set; }
        public string? BillId { get; set; }
        public short DomesticUnit { get; set; }
        public short? CommercialUnit { get; set; }
        public short? OtherUnit { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public bool? SpecialCustomer { get; set; }
        public bool? CommonSiphon { get; set; }
    }
}
