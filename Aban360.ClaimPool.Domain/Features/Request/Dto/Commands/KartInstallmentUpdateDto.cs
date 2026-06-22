namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartInstallmentUpdateDto
    {
        public int ZoneId { get; set; }
        public string StringTrackNumber { get; set; }

        public float CashPercent { get { return (InstallmentPercent / 100.0f); } }
        public float UncashPercent { get { return 1f - CashPercent; } }
        public long FirstInstallment { get; set; }
        public long InstallmentPercent { get; set; }
        public int InstallmentCount { get; set; }
        public long Installment { get; set; }

        public string InsertedBy { get; set; }
        public int Operator { get; set; }
    }
}
