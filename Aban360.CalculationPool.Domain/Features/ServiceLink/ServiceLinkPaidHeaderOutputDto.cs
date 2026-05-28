using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkPaidHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public long Amount { get; set; }

        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
