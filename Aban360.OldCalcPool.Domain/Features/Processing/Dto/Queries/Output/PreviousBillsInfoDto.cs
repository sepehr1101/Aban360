namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record PreviousBillsInfoDto
    {
        public string RegisterDateJalali { get; set; }
        public float ConsumptionAverage { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyCount { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
    }
}
