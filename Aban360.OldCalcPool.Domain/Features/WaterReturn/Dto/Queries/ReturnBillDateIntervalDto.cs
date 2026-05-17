namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillDateIntervalDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public ReturnBillDateIntervalDto(int zoneId, int customerNumber, string fromDateJalali, string toDateJalali)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
        public ReturnBillDateIntervalDto()
        {
        }
    }
}
