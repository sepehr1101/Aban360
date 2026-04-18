namespace Aban360.Common.BaseEntities
{
    public record MoshtrakInfoGetDto
    {
        public int DiscountCount { get; set; }
        public int DiscountId { get; set; }
        public string? DiscountTitle { get; set; }
        public string? BlockCode { get; set; }
    }
}
