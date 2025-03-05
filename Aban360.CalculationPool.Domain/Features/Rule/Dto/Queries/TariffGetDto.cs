namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record TariffGetDto
    {
        public int Id { get; set; }
        public short LineItemTypeId { get; set; }
        public string LineItemTypeTitle { get; set; }
        public short OfferingId { get; set; }
        public string OfferingTitle { get; set; }
        public string Condition { get; set; } = null!;
        public string Formula { get; set; } = null!;
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public string? Description { get; set; }
    }
}
