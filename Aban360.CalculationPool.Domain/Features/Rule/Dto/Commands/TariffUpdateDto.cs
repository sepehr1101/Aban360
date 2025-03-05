namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffUpdateDto
    {
        public int Id { get; set; }
        public short LineItemTypeId { get; set; }
        public short OfferingId { get; set; }
        public string Condition { get; set; } = null!;
        public string Formula { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public string? Description { get; set; }
    }
}
