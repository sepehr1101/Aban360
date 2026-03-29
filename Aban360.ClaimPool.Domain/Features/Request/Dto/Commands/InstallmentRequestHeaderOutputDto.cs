namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record InstallmentRequestHeaderOutputDto
    {
        public long Amount { get; set; }
        public int InstallmentCount { get; set; }
        public InstallmentRequestHeaderOutputDto(long amount,int installmentCount)
        {
            Amount = amount;
            InstallmentCount = installmentCount;
        }
        public InstallmentRequestHeaderOutputDto()
        {
        }
    }
}
