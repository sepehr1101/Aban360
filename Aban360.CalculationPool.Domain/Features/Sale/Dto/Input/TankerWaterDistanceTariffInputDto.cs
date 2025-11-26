namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterDistanceTariffInputDto
    {
        public short Id { get; set; }
        public short FromDistance { get; set; }
        public short ToDistance { get; set; }
        public long Amount { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

    }
}
