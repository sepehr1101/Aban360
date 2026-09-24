namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record ContractRepairDataOutputDto
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

        public string ChangeDateJalali { get; set; }
        public string UsageTitle { get; set; }
        public int TotalUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public string PreviousUsageTitle { get; set; }
        public int PreviousTotalUnit { get; set; }
        public int PreviousContractualCapacity { get; set; }
        public long Amount  { get; set; }
        public string ItemTitle  { get; set; }
        public int ItemId{ get; set; }
    }
}
