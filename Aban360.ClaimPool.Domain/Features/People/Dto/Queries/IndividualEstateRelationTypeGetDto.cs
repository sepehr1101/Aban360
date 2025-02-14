using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualEstateRelationTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();
    }
}
