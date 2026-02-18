namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentDataOutputDto
    {
        public string DeadLineDateJalali { get; set; }
        public long Payable { get; set; }
        public int QueueNumber { get; set; }

    }
}
