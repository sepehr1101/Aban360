using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerCalculationResultOutputDto
    {
        private const decimal _vatRate = 0.1m;
        public int CustomerNumber { get; set; }
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
        public string? MobileNumber { get; set; }
        public decimal Tax => (Water + Budget) * _vatRate;
        public decimal Water { get; set; }
        public decimal Delivery { get; set; }
        public decimal Budget { get; set; }
        public decimal Final => Water + Tax + Delivery + Budget;
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string SaleStateTitle { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public int Consumption { get; set; }

    }
}
