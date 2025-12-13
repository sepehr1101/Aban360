namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record ManualBillHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali  { get; set; }
        public float ConumptionAverage { get; set; }
    }
}