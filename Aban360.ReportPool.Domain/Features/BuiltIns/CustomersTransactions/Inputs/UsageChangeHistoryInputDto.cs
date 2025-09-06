namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record UsageChangeHistoryInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public ICollection<int>? ZoneIds { get; set; }
        public ICollection<int> FromUsageIds { get; set; }
        public ICollection<int> ToUsageIds { get; set; }

    }
}
