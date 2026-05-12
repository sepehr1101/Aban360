using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record UnAssessmentHeaderOutputDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public UnAssessmentHeaderOutputDto(int recordCount,string title)
        {
            RecordCount = recordCount;
            Title = title;
        }
        public UnAssessmentHeaderOutputDto()
        {
        }
    }
}