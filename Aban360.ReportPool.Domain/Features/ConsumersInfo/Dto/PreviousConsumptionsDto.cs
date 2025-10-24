namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record PreviousConsumptionsDto
    {
        public double  ConsumptionAmount { get; set; }
        public string ConsumptionDateJalali { get; set; }
    }
}
