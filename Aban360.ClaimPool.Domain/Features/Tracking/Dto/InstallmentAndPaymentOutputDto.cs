namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record InstallmentAndPaymentOutputDto
    {
        public long Amount { get; set; }
        public string DueDateJalali { get; set; }
        public string PayId { get; set; }
    }
}