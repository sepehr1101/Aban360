namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record SetExaminationResultDataDto
    {
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDayJalali { get; set; }
        public string FullName { get; set; }
        public int TrackNumber { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string AssessmentResultTitle { get; set; }
        public bool IsNewBranch { get; set; }
        public bool IsNewSewage { get; set; }
        public bool IsResultSuccess { get; set; }

    }
}