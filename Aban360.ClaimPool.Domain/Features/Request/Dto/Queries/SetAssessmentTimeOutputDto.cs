namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record SetAssessmentTimeOutputDto
    {
        public int TrackNumber { get; set; }
        public bool HasAssessmentSms { get; set; }
        public bool HasCustomerSms { get; set; }
        public string? AssessmentMessage { get; set; }
        public string? CustomerMessage { get; set; }
        public SetAssessmentTimeOutputDto(int trackNumber, bool hasAssessmentSms, bool hasCustomerSms, string? assessmentMessage, string? customerMessage)
        {
            TrackNumber = trackNumber;
            HasAssessmentSms = hasAssessmentSms;
            HasCustomerSms = hasCustomerSms;
            AssessmentMessage = assessmentMessage;
            CustomerMessage = customerMessage;
        }
    }
}
