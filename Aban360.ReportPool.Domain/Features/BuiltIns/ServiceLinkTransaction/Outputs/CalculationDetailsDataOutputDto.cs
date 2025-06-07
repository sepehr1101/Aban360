namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record CalculationDetailsDataOutputDto
    {
        public ICollection<CalculationDetialItemTitleDto> ItemTitles { get; set; }
}
}
