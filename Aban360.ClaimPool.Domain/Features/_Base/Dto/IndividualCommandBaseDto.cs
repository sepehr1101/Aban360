using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features._Base.Dto
{
    public record IndividualCommandBaseDto
    {
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public short IndividualTypeId { get; set; }
        public IndividualEstateRelationTypeEnum IndividualEstateRelationTypeId { get; set; }
        public ICollection<short>? TagIds { get; set; }
    }
}
