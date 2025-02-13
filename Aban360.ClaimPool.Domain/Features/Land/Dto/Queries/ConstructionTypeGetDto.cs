using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConstructionTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();
    }
}
