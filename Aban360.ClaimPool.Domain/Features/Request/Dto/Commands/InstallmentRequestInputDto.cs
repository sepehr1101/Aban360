namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record InstallmentRequestInputDto
    {
        public int TrackNumber { get; set; }
        public int InstallmentCount { get; set; }
        public int PrepaymentPercent { get; set; }
        public bool IsConfirm { get; set; }
    }
}
