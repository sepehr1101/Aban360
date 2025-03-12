namespace Aban360.ReportPool.Domain.Features.Dto
{
    public record PreviousConsumptionsDto
    {
        public int ConsumptionAmount { get; set; }
        public string ConsumptionDateJalali { get; set; }
    }
}
