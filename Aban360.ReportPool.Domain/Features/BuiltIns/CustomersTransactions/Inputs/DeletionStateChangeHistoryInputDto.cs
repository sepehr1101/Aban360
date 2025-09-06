namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record DeletionStateChangeHistoryInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }

        public ICollection<int>? ZoneIds { get; set; }
        public ICollection<int> FromDeletionStateIds { get; set; }
        public ICollection<int> ToDeletionStateIds { get; set; }

    }
}
