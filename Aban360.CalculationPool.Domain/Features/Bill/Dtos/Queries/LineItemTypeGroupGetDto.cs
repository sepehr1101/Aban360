namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record LineItemTypeGroupGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ImpactSign { get; set; }
        public string? Description { get; set; }
    }
}
