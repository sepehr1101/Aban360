namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record CalculationDetailsDataOutputDto
    {
        public ICollection<CalculationDetialItemTitleDto> ItemTitles { get; set; }
}

public record CalculationDetialItemTitleDto
{
    public string ServiceType { get; set; }
    public string Amount { get; set; }
    public string Discount { get; set; }
}
}
