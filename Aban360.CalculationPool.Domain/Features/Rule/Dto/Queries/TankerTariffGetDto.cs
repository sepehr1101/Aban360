namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries
{
    public record TankerTariffGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string WaterFormula { get; set; }
    }
}