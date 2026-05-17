namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BedBesItemsOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string PayId { get; set; }
        public long SumItems { get; set; }
        public long Tax { get; set; }
        public long Water { get; set; }
        public long Budget { get; set; }
    }
}
