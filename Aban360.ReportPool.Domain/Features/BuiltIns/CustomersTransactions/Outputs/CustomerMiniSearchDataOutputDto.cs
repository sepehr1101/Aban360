namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerMiniSearchDataOutputDto
    {
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? FullName { get; set; }
        public string? FatherName { get; set; }
        public string? RegionTitle { get; set; }
        public string? ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public int? CustomerNumber { get; set; }
        public string? BillId { get; set; }
        public string? ReadingNumber { get; set; }
        public string? UsageSellTitle { get; set; }
        public string? UsageConsumptionTitle { get; set; }
        public string? BranchTypeTitle { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? NationalCode { get; set; }
        public short? CommercialUnit { get; set; }
        public short DomesticUnit { get; set; }
        public short? OtherUnit { get; set; }
        public short? TotalUnit { get; set; }
        public short? FamilyCount { get; set; }
        public short? EmptyUnit { get; set; }
        public string? DeletionStateTitle { get; set; }
        public string? DiscountTypeTitle { get; set; }
        public string? HouseholdDateJalali{ get; set; }
        public bool? HasSewage { get; set; }
        public string? GuildTitle { get; set; }
        public string? SiphonDiameterTitle { get; set; }
        public string? MeterDiameterTitle { get; set; }
        public bool? SpecialCustomer { get; set; }
        public bool? CommonSiphon { get; set; }
    }
}
