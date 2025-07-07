namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterCalculationDetailsDataOutputDto
    {
        public ICollection<WaterCalculationDetailItemTitleDto> ItemTitles { get; set; }
        public long SumItems { get; set; }
        public long SumOffItems { get; set; }
        public long Payble { get; set; }
    }
}
