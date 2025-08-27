namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterComparisonBatchInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int ZoneId { get; set; }
    }

}
