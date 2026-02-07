namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record AmountConfirmedOutputDto
    {
        public IEnumerable<OfferingAmountOutputDto> Offerings { get; set; }
        public long OfferingAmount { get; set; }
        public long OfferingDiscount { get; set; }
        public long OfferingPayable { get; set; }

        public IEnumerable<InstallmentAndPaymentOutputDto> IstallmentsAndPayments { get; set; }
        public long IstallmentAndPaymentAmount { get; set; }
    }
}