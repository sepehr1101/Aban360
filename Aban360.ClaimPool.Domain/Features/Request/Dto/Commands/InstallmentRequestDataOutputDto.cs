namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record InstallmentRequestDataOutputDto
    {
        public long Amount { get; set; }
        public string DueDateJalali { get; set; }
        public string? PaymentId { get; set; }
        public InstallmentRequestDataOutputDto(long amount, string dueDateJalali, string? paymentId)
        {
            Amount= amount; 
            DueDateJalali= dueDateJalali;
            PaymentId=PaymentId;
        }
        public InstallmentRequestDataOutputDto()
        {
        }
    }
}
