namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MeterChangeRandomInputDto
    {
        public string ChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int ChangeCount { get; set; }
    }
}
