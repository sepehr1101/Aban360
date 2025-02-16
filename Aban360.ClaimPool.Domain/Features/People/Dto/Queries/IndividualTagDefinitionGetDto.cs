using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualTagDefinitionGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
        public virtual ICollection<IndividualTag> IndividualTags { get; set; } = new List<IndividualTag>();
    }
}
