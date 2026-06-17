using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillTransactionDetailHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string FullName { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public string? LatestMeterChangeDateJalali { get; set; }

        public string? PreviousMeterDateJalali { get; set; }
        public int PreviousMeterNumber { get; set; }
    }
}
