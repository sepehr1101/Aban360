using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record AssessmentSuccessResultDataOutputDto
    {
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public string NeighbourBillId { get; set; }
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string RegionTitle { get; set; }
        public int RegionId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Caller { get; set; }
        public string NotificationMobile { get; set; }
        public string Address { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDayJalali { get; set; }
        public string AssessmentResultTitle { get; set; }
        public bool HasTrench { get; set; }

        public ICollection<MoshtrakCompanyService> CompanyServices{ get; set; }
    }
}