using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualEstateRelationTypeGetDto
    {
        public IndividualEstateRelationEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
