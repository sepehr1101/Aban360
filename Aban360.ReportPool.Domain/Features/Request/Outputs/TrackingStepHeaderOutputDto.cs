using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record TrackingStepHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public int RecordCount { get; set; }
        public int RequestCount { get; set; }
    }
}
