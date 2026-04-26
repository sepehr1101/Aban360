namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record AssessmentSetTimeInputDto
    {
        public int TrackNumber { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentDateJalali { get; set; }
        public string? Description { get; set; }
        public bool HasCustomerSms { get; set; }
        public bool HasAssessmentSms { get; set; }
    }
}
