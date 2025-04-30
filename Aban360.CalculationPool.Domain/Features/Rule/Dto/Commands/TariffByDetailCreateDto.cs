namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffByDetailCreateDto
    {
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public short UsageId { get; set; }
        public short IndividualTypeId { get; set; }
        public string FromBillId { get; set; } = null!;
        public string ToBillId { get; set; } = null!;
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
        public string? Description { get; set; }
    }
}
