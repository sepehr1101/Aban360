using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record JudicalNoticeCommandHeaderOutputDto
    {
        public string ZoneTitle { get; set; }
        public string RegionTitle { get; set; }
        public string BillId { get; set; }
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
