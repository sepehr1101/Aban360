namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record RequestBillDetailGetDto
    {
        public int Id { get; set; }
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public long Amount { get; set; }
        public long FinalAmount { get; set; }
        public long DiscountAmount { get; set; }
        public string DiscountTypeTitle { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public int TypeCode { get; set; }
        public string TypeTitle { get; set; }

    }
}
