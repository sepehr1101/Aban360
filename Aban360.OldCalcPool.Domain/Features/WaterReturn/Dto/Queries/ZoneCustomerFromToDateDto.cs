namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ZoneCustomerFromToDateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ZoneCustomerFromToDateDto(int zoneId,int customerNumber,string fromDate,string toDate)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }

}
