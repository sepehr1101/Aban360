using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerMiniSearchHeaderOutputDto
    {
        public CustomerMiniSearchInputEnum SearchType { get; set; }
        public string Input { get; set; }

        public string  ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string Title { get; set; }

    }
}
