namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterDiscountByTypeDetailInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public IEnumerable<int> DiscountTypeIds { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
