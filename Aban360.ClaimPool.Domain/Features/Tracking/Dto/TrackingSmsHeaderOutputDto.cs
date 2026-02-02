using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingSmsHeaderOutputDto
    {
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public TrackingSmsHeaderOutputDto(string title, int recordCount)
        {
            Title = title;
            RecordCount = recordCount;
        }
    }
}
