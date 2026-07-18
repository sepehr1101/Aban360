namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalDetailDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string BillId { get; set; }
        public string FullName { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string? MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string NationalCode { get; set; }
    }
}
