namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record PreviousConsumptionsDto
    {
        public int ConsumptionAmount { get; set; }
        public string ConsumptionDateJalali { get; set; }
    }
}
