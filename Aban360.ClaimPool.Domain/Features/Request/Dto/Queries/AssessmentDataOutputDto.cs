namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentDataOutputDto
    {
        public Guid Id { get; set; }
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDateJalali { get; set; }
        public DateTime AssessmentGregorianDateTime { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string? ResultTitle { get; set; }
        public int? ResultId { get; set; }
        public DateTime? SetResultDateTime { get; set; }
        public string? Description { get; set; }
        public Guid TrackId { get; set; }
        public Guid? TrackIdResult { get; set; }//todo : check


        public string? X1 { get; set; }
        public string? Y1 { get; set; }
        public string? X2 { get; set; }
        public string? Y2 { get; set; }
        public string? Accuracy { get; set; }
        
        public string ReadingNumber { get; set; }
        public int Premises { get; set; }
        public int HouseValue { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string? AllInJson { get; set; }
    }
}