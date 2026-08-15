using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record BasicInfoChangeHistoryInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public IEnumerable<int> ZoneIds { get; set; }
        public CustomerBasicPropertyEnum ItemChange { get; set; }

    }
}
