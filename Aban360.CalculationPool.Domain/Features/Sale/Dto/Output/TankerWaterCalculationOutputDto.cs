namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
   public record TankerWaterCalculationOutputDto
    {
        public decimal TaxAmount { get; set; }
        public decimal WaterAmountWithoutTax { get; set; }
        public decimal WaterAmountWithTax { get; set; }
        public decimal DeliveryAmount { get; set; }

        public decimal FinalAmount { get; set; }
    }
}
