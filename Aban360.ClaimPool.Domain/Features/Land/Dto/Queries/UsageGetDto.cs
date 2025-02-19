using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
        public virtual ICollection<Estate> EstateUsageConsumtions { get; set; } = new List<Estate>();

        public virtual ICollection<Estate> EstateUsageSells { get; set; } = new List<Estate>();
    }
}
