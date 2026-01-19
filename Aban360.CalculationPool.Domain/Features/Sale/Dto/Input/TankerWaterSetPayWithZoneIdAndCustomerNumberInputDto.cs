namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto
    {
        public string PaymentId { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto(string paymentId, int zoneId, int customerNumber)
        {
            PaymentId = paymentId;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
    }
}
