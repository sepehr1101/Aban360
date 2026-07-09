using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkUnconfirmedHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string? FullName { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public long Amount { get; set; }
    }
}
