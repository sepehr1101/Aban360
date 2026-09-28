using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record ToSetAssessmentTimeGetDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public string NeighbourBillId { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string NotificationNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

        public IEnumerable<StringDictionary> AssessmentsList { get; set; }
        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }
    }
}
