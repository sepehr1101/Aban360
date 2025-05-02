namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record CapacityCalculationIndexGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
