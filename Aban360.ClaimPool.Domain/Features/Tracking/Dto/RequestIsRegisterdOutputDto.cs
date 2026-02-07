using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record RequestIsRegisterdOutputDto
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
        public string MessageNumber { get; set; }
        public string Address { get; set; }
        public ICollection<NumericDictionary> CompanyServiceSelected { get; set; }
    }
}