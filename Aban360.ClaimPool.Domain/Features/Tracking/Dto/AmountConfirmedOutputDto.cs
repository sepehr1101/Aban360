using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record AmountConfirmedOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string BillId { get; set; }
        public IEnumerable<NumericDictionary> ServiceSelected { get; set; }

        public IEnumerable<OfferingAmountOutputDto> Offerings { get; set; }
        public long OfferingAmount { get; set; }
        public long OfferingDiscount { get; set; }
        public long OfferingPayable { get; set; }

        public IEnumerable<InstallmentRequestDataOutputDto> IstallmentsAndPayments { get; set; }
        public long IstallmentAndPaymentAmount { get; set; }
    }
}   