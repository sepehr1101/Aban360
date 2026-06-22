namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record InstallmentRequestDataOutputDto
    {
        public long Amount { get; set; }
        public string DueDateJalali { get; set; }
        public string? PaymentId { get; set; }
        public int Queue { get; set; }
        public InstallmentRequestDataOutputDto(long amount, string dueDateJalali, string? paymentId,int queue)
        {
            Amount = amount;
            DueDateJalali = dueDateJalali;
            PaymentId = paymentId;
            Queue = queue;
        }
        public InstallmentRequestDataOutputDto()
        {
        }
    }
}
