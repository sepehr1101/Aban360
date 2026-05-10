namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
