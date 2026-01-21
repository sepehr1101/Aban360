namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record AfterSaleDataOutputDto
    {
        public IEnumerable<SaleDataOutputDto> PreviousValue { get; set; }
        public IEnumerable<SaleDataOutputDto> CurrentValue { get; set; }
        public IEnumerable<SaleDataOutputDto> DifferentValue { get; set; }
        public AfterSaleDataOutputDto(IEnumerable<SaleDataOutputDto> previousValue, IEnumerable<SaleDataOutputDto> currentValue, IEnumerable<SaleDataOutputDto> differentValue)
        {
            PreviousValue = previousValue;
            CurrentValue = currentValue;
            DifferentValue = differentValue;
        }
    }
}
