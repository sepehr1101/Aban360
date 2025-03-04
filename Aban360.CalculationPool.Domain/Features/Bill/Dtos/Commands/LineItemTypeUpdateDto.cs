namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record LineItemTypeUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public short LineItemTypeGroupId { get; set; }
    }
}
