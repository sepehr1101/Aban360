namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto(int id, int zoneId, int customerNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
    }
}
