namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record UnAssessmentDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string BillId { get; set; }
        public int TrackNumber { get; set; }
        public Guid TackId { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public string AssessmentName { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDateJalali { get; set; }

    }
}