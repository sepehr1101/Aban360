namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsConfirmFileOutputDto
    {
        public long BillCount { get; set; }
        public long CorrectBillCount { get; set; }
        public long WarningBillCount { get; set; }
    }
}
