namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record NonPermanentBranchInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        
        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
    }
}
