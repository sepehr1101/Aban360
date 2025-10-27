namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record SaleInputDto
    {
        public short WaterDiameterId { get; set; }
        public short? SiphonDiameterId { get; set; }

        public int ZoneId { get; set; }
        public string? Block { get; set; }
        public bool IsDomestic { get; set; }
    }
}
