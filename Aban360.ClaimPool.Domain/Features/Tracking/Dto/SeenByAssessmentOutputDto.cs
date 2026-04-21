namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record SeenByAssessmentOutputDto
    {
        public int TrackNumber { get; set; }
        public Guid AssessmentTrackId { get; set; }
        public string? BillId { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDayJalali { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ResultId { get; set; }
        public string? ResultTitle { get; set; }
        public string? ResultDescription { get; set; }
        public Guid? TrackIdResult { get; set; }
        public DateTime? SetResultDateTime { get; set; }
        public string? X1 { get; set; }
        public string? Y1 { get; set; }
        public string? X2 { get; set; }
        public string? Y2 { get; set; }

    }
}