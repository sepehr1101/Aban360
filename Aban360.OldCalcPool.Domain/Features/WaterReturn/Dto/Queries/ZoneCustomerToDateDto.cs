namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ZoneCustomerToDateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ToDate { get; set; }
        public ZoneCustomerToDateDto(int zoneId,int customerNumber,string toDate)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            ToDate = toDate;
        }
    }
}
