namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record MembersDebtAmountUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public long Amount { get; set; }
        public MembersDebtAmountUpdateDto(int zoneId, int customerNumber, string billId, long amount)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            BillId = billId;
            Amount = amount;
        }
        public MembersDebtAmountUpdateDto()
        {
        }
    }
}
