namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsGetDataToSendInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public CollectBillsGetDataToSendInputDto(IEnumerable<int> zoneIds, string fromDateJalali, string toDateJalali)
        {
            ZoneIds = zoneIds;
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
        public CollectBillsGetDataToSendInputDto()
        {
        }
    }
}
