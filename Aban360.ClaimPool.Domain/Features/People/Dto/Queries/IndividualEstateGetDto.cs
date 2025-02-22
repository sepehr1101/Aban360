using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualEstateGetDto
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int EstateId { get; set; }
        public IndividualEstateRelationEnum IndividualEstateRelationTypeId { get; set; }
    }
}
