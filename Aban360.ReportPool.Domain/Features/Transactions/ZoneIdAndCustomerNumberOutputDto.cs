namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record ZoneIdAndCustomerNumberOutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public ZoneIdAndCustomerNumberOutputDto(int zoneId,int customerNumber)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
        public ZoneIdAndCustomerNumberOutputDto()
        {

        }
    }
}
