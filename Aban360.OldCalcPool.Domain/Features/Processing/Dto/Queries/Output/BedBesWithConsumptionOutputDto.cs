namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BedBesWithConsumptionOutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int previousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int Consumption { get; set; }
        public float  ConsumptionAverage { get; set; }
    }
}
