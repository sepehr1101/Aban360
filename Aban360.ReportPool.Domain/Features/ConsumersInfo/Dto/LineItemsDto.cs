namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record LineItemsDto
    {
        public string Item { get; set; }
        public long Amount { get; set; }
    }
}
