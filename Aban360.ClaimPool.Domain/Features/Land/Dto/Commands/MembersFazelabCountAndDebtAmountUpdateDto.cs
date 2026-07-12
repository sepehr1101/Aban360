namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record MembersFazelabCountAndDebtAmountUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public long Amount { get; set; }
        public string ToDateJalali { get; set; }
        public MembersFazelabCountAndDebtAmountUpdateDto(int zoneId, int customerNumber, string billId, long amount, string toDateJalali)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            Amount = amount;
            ToDateJalali = toDateJalali;
        }
        public MembersFazelabCountAndDebtAmountUpdateDto()
        {
        }
    }
}
