using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectDetailHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
    }
}
