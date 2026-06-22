namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakInstallmentUpdateDto
    {
        public int TrackNumber { get; set; }
        public int PrePaymentPrecent { get; set; }
        public int InstallmentCount { get; set; }
        public MoshtrakInstallmentUpdateDto(int trackNumber, int prepaymentPercent, int insallmentCount)
        {
            TrackNumber = trackNumber;
            PrePaymentPrecent = prepaymentPercent;
            InstallmentCount = insallmentCount;
        }
        public MoshtrakInstallmentUpdateDto()
        {
        }
    }
}
