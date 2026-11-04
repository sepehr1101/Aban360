namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentDuplicateDetailDataOutputDto
    {
        public string DuplicateItem { get; set; }
        public int ZoneId { get; set; }
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public string NeighbourBillId { get; set; }
        public string NotificationMobile { get; set; }
        public string InsertDateJalali { get; set; }
        public int PeriodCount { get; set; }
        public bool IsColour { get; set; }

    }
}
