using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerInsertInputDto
    {
        public int ZoneId { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public int Consumption { get; set; }
        public int Distance { get; set; }
        public TankerWaterSaleStateEnum SaleState { get; set; }

        public bool IsConfirm { get; set; }
        public bool HasSms { get; set; }
    }
}
