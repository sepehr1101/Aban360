namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CapacityCalculationIndexCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
