namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsGetDataToSendInputDto
    {
        public IEnumerable<DbNameAndZoneIdDto> ZoneInfo { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public CollectBillsGetDataToSendInputDto(IEnumerable<DbNameAndZoneIdDto> zoneInfo, string fromDateJalali, string toDateJalali)
        {
            ZoneInfo = zoneInfo;
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
        public CollectBillsGetDataToSendInputDto()
        {
        }
    }
}
