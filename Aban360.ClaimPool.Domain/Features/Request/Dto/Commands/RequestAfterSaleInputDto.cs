namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record RequestAfterSaleInputDto
    {
        public string BillId { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string? BlockCode { get; set; }
        public int DiscountTypeId { get; set; }
        public int DiscountCount { get; set; }
        public string? Description { get; set; }
        public ICollection<int> SelectedServices { get; set; }
    }
}
