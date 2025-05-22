using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestIndividualGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public short IndividualTypeId { get; set; }
        public IndividualEstateRelationTypeEnum IndividualEstateRelationTypeId { get; set; }
        public ICollection<RequestIndividualTagGetDto>? Tags { get; set; }
        public ICollection<RequestIndividualDiscountTypeGetDto> IndividualDiscountTypes { get; set; }

    }
}
