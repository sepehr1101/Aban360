namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsGetDataToSendInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public CollectBillsGetDataToSendInputDto( string fromDateJalali, string toDateJalali)
        {
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
        public CollectBillsGetDataToSendInputDto()
        {
        }
    }
}
