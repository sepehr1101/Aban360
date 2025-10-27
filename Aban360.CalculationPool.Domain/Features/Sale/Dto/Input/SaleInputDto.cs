namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record SaleInputDto
    {
        public int WaterDiameterId { get; set; }
        public int SiphonDiameterId { get; set; }

        public int ZoneId { get; set; }
        public string? Block { get; set; }
        public bool IsDomestic { get; set; }
    }
}
