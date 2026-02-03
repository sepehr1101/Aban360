namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record OfferingAmountOutputDto
    {
        public string Title { get; set; }
        public long Amount { get; set; }
        public long Discount { get; set; }
    }
}