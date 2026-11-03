using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentDuplicatHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
