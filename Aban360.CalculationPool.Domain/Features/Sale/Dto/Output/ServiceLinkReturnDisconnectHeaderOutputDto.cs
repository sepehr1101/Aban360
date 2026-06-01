using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record ServiceLinkReturnDisconnectHeaderOutputDto
    {
        public string BillId { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int OfferingCount { get; set; }
        public long Amount { get; set; }
        public ServiceLinkReturnDisconnectHeaderOutputDto(string billId,string title,int offeringCount,long amount)
        {
            BillId = billId;
            Title = title;
            OfferingCount = offeringCount;
            Amount = amount;
        }
        public ServiceLinkReturnDisconnectHeaderOutputDto()
        {
        }
    }
}
