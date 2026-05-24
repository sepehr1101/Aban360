using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record ServiceLinkReturnInputDto
    {
        public string BillId { get; set; }
        public IEnumerable<ServiceLinkReturnItemDto> ReturnItems { get; set; }
    }
    public record ServiceLinkReturnItemDto
    {
        public int AmountItemId { get; set; }
        public long Amount { get; set; }
        public ServiceLinkReturnCategoryTypeEnum CategoryType { get; set; }
    }
}
