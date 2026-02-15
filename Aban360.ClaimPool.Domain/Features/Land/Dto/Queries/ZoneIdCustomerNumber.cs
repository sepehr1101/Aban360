namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ZoneIdCustomerNumber
    {
        public int ZoneId { get; set; }
        public string CustomerNumber { get; set; }
        public ZoneIdCustomerNumber(int zoneId, string customerNumber)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
        public ZoneIdCustomerNumber()
        {

        }
    }
}
