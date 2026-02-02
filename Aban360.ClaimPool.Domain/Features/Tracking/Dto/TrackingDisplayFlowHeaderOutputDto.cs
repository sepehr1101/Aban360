namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDisplayFlowHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }

        public string TrackNumber { get; set; }
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }
    }
}
