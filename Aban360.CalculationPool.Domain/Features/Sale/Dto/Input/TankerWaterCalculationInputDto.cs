using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterCalculationInputDto
    {
        public int ZoneId { get; set; }
        public int Distance { get; set; }
        public int Consumption { get; set; }
        public TankerWaterSaleStateEnum  SaleState{ get; set; }
        public bool IsConfirm { get; set; }
    }
}
