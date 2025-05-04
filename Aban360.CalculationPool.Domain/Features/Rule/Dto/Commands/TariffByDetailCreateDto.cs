namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffByDetailCreateDto
    {
        public string FromDate{ get; set; } = null!;
        public string? ToDate{ get; set; } 
        public short UsageId { get; set; }
        public short IndividualTypeId { get; set; }
        public short Handover { get; set; }
        public string FromReadingNumber{ get; set; } = null!;
        public string ToReadingNumber{ get; set; } = null!;
        public int ZoneId { get; set; }
    }
}
