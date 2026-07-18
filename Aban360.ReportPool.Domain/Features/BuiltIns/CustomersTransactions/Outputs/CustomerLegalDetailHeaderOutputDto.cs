using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerLegalDetailHeaderOutputDto
    {
        public int ZoneCount { get; set; }
        public int CustomerCount { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
