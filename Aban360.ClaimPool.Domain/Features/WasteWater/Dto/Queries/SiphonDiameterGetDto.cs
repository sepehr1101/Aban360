using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries
{
    public record SiphonDiameterGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<Siphon> Siphons { get; set; } = new List<Siphon>();
    }
}
