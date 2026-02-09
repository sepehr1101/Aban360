using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record AssessmentResultUpdateDto
    {
        public int UsageId { get; set; }
        public int BranchTypeId { get; set; }
        public int DiscountTypeId { get; set; }
        public int TrackingResultId { get; set; }
        public int MeterDiameterId { get; set; }
        public int SiphonDiameterId { get; set; }


        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();

    }
}
