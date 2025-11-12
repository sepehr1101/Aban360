namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record MalfunctionToChangeInputDto
    {
        public  ICollection<int> ZoneIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
