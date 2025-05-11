namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffCreateDto
    {
        public short LineItemTypeId { get; set; }
        public string Title { get; set; } = null!;
        public short OfferingId { get; set; }
        public string Condition { get; set; } = null!;
        public string Formula { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public string? Description { get; set; }
    }
}
