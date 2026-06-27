using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillLatestListHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
    }
}
