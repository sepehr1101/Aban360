namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record PreviousConsumptionsDto
    {
        public float ConsumptionAverage { get; set; }
        public string RegisterDateJalali { get; set; }
    }
}
