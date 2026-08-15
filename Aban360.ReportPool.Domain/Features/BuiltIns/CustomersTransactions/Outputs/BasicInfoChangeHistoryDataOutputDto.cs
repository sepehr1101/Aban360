namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record BasicInfoChangeHistoryDataOutputDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string BillId { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }

        public string ChangeDateJalali { get; set; }
        public string FromItem { get; set; }
        public string ToItem { get; set; }
    }
}
