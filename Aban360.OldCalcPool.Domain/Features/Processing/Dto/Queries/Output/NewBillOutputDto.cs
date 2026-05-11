namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record NewBillOutputDto
    {
        public AbBahaCalculationDetails AbBahaCalculationDetail { get; set; }
        public IEnumerable<PreviousConsumptionsDto> PreviousConsumption { get; set; }
        public int? PreviousMeterNumber { get; set; }
        public string? PreviousReadingDateJalali { get; set; }
        public string? PreviousMeterChangeDateJalali { get; set; }
    }
}
