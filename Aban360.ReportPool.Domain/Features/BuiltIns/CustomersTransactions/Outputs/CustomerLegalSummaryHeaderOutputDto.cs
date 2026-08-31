using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalSummaryHeaderOutputDto
    {
        public int CustomerCount { get; set; }
        public int ValidLegalCount { get; set; }
        public int InValidLegalCount { get; set; }
        public int ValidNaturalCount { get; set; }
        public int InValidNaturalCount { get; set; }
        public int InvalidCount { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
