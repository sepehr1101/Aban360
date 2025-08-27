namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MalfunctionToChangeInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
