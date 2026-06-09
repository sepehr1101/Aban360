using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkReturnInputDto
    {
        public string BillId { get; set; } = default!;
        public int AmountItemId { get; set; }
        public long Amount { get; set; }
        public int DiscountTypeId { get; set; }
        public int ReturnCodeId { get; set; }
        public ServiceLinkReturnCategoryTypeEnum CategoryType { get; set; }
    }
}
