namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record SaleInputDto
    {
        public short WaterDiameterId { get; set; }
        public short? SiphonDiameterId { get; set; }

        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public string? Block { get; set; }
        public bool IsDomestic { get; set; }
        public int? DiscountTypeId { get; set; }
        public int? DiscountCount { get; set; }
        public bool? HasWaterBroker { get; set; }
        public int ContractualCapacity { get; set; }
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int OtherUnit { get; set; }
    }
}
