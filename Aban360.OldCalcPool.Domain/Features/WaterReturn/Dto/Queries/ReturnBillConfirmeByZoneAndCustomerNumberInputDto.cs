namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillConfirmeByZoneAndCustomerNumberInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int JalaseNumber { get; set; }
        public ReturnBillConfirmeByZoneAndCustomerNumberInputDto(int zoneId, int customerNumber, int jalaseNumber)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            JalaseNumber = jalaseNumber;
        }
    }
}
