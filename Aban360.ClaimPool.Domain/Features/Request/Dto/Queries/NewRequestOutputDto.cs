namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record NewRequestOutputDto
    {
        public int TrackNumber { get; set; }
        public bool HasTrackNumberSms { get; set; }
        public string? TrackNumberMessage { get; set; }
        public bool HasCustomerSms { get; set; }
        public string? CustomerMessage { get; set; }
        public bool HasAssessmentSms { get; set; }
        public string? AssessmentMessage { get; set; }
        public string? AssessmentDateJalali { get; set; }
        public string? AssessmentName { get; set; }
        public NewRequestOutputDto(int trackNumber, bool hasTrackNumberSms, string? trackNumberMessage, bool hasCustomerSms, string? customerMessage, bool hasAssessmentSms, string? assessmentMessage, string? assessmentDateJalali, string? assessmentName)
        {
            TrackNumber = trackNumber;
            HasTrackNumberSms = hasTrackNumberSms;
            TrackNumberMessage = trackNumberMessage;
            HasCustomerSms = hasCustomerSms;
            CustomerMessage = customerMessage;
            HasAssessmentSms = hasAssessmentSms;
            AssessmentMessage = assessmentMessage;
            AssessmentDateJalali = assessmentDateJalali;
            AssessmentName = assessmentName;
        }
        public NewRequestOutputDto()
        {
        }
    }
}