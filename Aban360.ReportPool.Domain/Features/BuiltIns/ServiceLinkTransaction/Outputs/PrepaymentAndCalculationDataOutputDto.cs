namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PrepaymentAndCalculationDataOutputDto
    {
        public ICollection<PrepaymentAndCalculationItemTitleDto> ItemTitles { get; set; }
    }
}
