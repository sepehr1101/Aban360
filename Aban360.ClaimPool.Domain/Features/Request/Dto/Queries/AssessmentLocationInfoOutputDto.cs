namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentLocationInfoOutputDto
    {
        public Guid TrackId { get; set; }
        public string MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string NotificationMobileNumber { get; set; }
        public string? BillId { get; set; }
        public string? NeighbourBillId { get; set; }
        public string StringTrackNumber { get; set; }
        public int TrackNumber { get; set; }
        public int CustomerNumber { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int DiscountCount { get; set; }
        public string NationalCode { get; set; }
        public string FatherName { get; set; }
        public int HouseValue { get; set; }
        public string? Description { get; set; }
        public bool IsNonPermanent { get; set; }
        public bool HasCustomerNumber { get; set; }
        public string FullName { get; set; }
        public bool IsVisited { get; set; }
        public string DiscountTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int? AssessmentCode { get; set; }
        public string? AssessmentDateJalali { get; set; }
        public string? AssessmentName { get; set; }
        public string? AssessmentMobileNumber { get; set; }

        public IEnumerable<ServiceGroupWithCheckedOutputDto> ServiceGroups { get; set; }
    }
}