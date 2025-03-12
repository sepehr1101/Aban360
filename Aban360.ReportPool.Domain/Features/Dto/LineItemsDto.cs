namespace Aban360.ReportPool.Domain.Features.Dto
{
    public record LineItemsDto
    {
        public string Item { get; set; }
        public long Amount { get; set; }
    }
}
