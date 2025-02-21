using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualEstateRelationTypeDeleteDto
    {
        public IndividualEstateRelationEnum Id { get; set; }
    }
}
