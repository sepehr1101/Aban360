using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record LatestCustomersInfoHeaderOutputDto
    {
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public int RecordCount { get; set; }
        public string Title { get; set; }
    }
}
