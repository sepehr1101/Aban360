namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record LineItemTypeGroupUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ImpactSignId { get; set; }
        public string? Description { get; set; }
    }
}
