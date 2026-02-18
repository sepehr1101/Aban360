namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentDataOutputDto
    {
        public string DeadLineDateJalali { get; set; } = default!;
        public long Payable { get; set; }
        public int QueueNumber { get; set; }
        public string QueueNumberTitle { get; set; }
        public string BillId { get; set; } = default!;
        public string PaymentId { get; set; } = default!;

    }
}
