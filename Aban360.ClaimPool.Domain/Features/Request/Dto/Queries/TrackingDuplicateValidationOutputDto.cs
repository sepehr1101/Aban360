using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingDuplicateValidationOutputDto
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } 
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string NationalCode { get; set; }
        public int TrackNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string NotificationNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public bool IsDuplicate { get; set; }

        public string? CertificateNumber { get; set; }
        public string?  Address { get; set; }
        public string? NeighbourBillId { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }

        public string RequestOrigin { get; set; }
        public int RequestOriginId { get; set; }
        public string LatestStatusTitle { get; set; }
        public int LatestStatusId { get; set; }

        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }
    }
}
