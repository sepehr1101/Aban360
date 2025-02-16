using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries
{
    public record WaterMeterTagDefinitionGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Color { get; set; }
        public virtual ICollection<WaterMeterTag> WaterMeterTags { get; set; } = new List<WaterMeterTag>();
    }
}
