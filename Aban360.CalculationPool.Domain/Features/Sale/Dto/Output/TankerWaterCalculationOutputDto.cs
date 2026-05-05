namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerWaterCalculationOutputDto
    {
        private const decimal _vatRate = 0.1m;
        public int CustomerNumber { get; set; }
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
        public string? MobileNumber { get; set; }
        public decimal Tax { get; }
        public decimal Water { get; }
        public decimal Delivery { get; }
        public decimal Budget { get; }
        public decimal Final { get; }

        public TankerWaterCalculationOutputDto(int? customerNumber,string? billId,string? paymentId,string? mobileNumber,decimal water, decimal budget, decimal delivery)
        {
            decimal tax = (water + budget) * _vatRate;

            CustomerNumber = customerNumber ?? 0;
            BillId = billId;
            PaymentId = paymentId;
            MobileNumber = mobileNumber;
            Tax = tax;
            Water = water;
            Delivery = delivery;
            Budget = budget;
            Final = tax + water + delivery + budget;
        }
    }
}
