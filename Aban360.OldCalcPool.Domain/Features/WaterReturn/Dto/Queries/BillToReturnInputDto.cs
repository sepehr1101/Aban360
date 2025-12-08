namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record BillToReturnInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public BillToReturnInputDto(int zoneid, int customerNumber, string fromDateJalali, string toDateJalali)
        {
            ZoneId = zoneid;
            CustomerNumber = customerNumber;
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
    }
}
