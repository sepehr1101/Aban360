namespace Aban360.ReportPool.Domain.Features.Dto
{
    public record LineItems
    {
        public string Item { get; set; }
        public long Amount { get; set; }
    }
}
