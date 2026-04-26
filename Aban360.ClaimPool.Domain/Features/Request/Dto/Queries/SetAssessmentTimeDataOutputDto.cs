namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record SetAssessmentTimeDataOutputDto
    {
        public Guid TrackId { get; set; }
        public int ServiceGroupId { get; set; }
        public int TrackNumber { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string? BillId { get; set; }
        public string ServiceSelectedList { get; set; }
        public string? NeighbourBillId { get; set; }
        public string? NeighbourAddress { get; set; }

        public string AssessmentName { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentMobileNumber { get; set; }
        public string AssessmentDateJalai { get; set; }
    }
}
