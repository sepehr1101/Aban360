namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record SetAssessmentTimeOutputDto
    {
        public bool HasAssessmentSms { get; set; }
        public bool HasCustomerSms { get; set; }
        public string? AssessmentMessage { get; set; }
        public string? CustomerMessage { get; set; }
        public SetAssessmentTimeOutputDto(bool hasAssessmentSms, bool hasCustomerSms, string? assessmentMessage, string? customerMessage)
        {
            HasAssessmentSms = hasAssessmentSms;
            HasCustomerSms = hasCustomerSms;
            AssessmentMessage = assessmentMessage;
            CustomerMessage = customerMessage;
        }
    }
}
