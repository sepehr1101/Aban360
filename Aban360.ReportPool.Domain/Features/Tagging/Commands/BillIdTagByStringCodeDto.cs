namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record BillIdTagByStringCodeDto
    {
        public string BillId { get; set; }
        public string StringCode { get; set; }
        public string? ExpireDateJalali { get; set; }
        public BillIdTagByStringCodeDto(string billId, string stringCode, string? expireDateJalali)
        {
            BillId = billId;
            StringCode = stringCode;
            ExpireDateJalali = expireDateJalali;
        }
        public BillIdTagByStringCodeDto()
        {
        }
    }
}
