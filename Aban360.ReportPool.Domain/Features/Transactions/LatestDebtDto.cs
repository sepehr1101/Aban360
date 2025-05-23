namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record LatestDebtDto
    {
        public string BillId { get; set; } = default!;
        public long WaterBillDebt { get; set; }
        public long ServiceLinkDebt { get; set; }

        public LatestDebtDto(string billId, long? waterBillDebt, long? serviceLinkDebt)
        {
            BillId = billId;
            WaterBillDebt = waterBillDebt ?? 0;
            ServiceLinkDebt = serviceLinkDebt ?? 0;
        }
    }
}
