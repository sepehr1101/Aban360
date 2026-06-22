using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record InstallmentRequestHeaderOutputDto
    {
        public long Amount { get; set; }
        public int InstallmentCount { get; set; }
        public int PrePaymentPercent { get; set; }
        public long PrePaymentAmount { get; set; }
        public long InstallmentAmount { get; set; }

        public int TrackNumber { get; set; }
        public string ServiceGroupTitle { get; set; }
        public string? BillId { get; set; }
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string FullName { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
