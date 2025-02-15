using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConstructionTypeCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
