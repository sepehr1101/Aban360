namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerWaterCalculationOutputDto
    {
        private const decimal _vatRate = 0.1m;
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
        public decimal Tax { get; }
        public decimal Water { get; }
        public decimal Delivery { get; }
        public decimal Budget { get; }
        public decimal Final { get; }

        public TankerWaterCalculationOutputDto(string? billId,string? paymentId,decimal water, decimal budget, decimal delivery)
        {
            decimal tax = (water + budget) * _vatRate;

            BillId = billId;
            PaymentId = paymentId;
            Tax = tax;
            Water = water;
            Delivery = delivery;
            Budget = budget;
            Final = tax + water + delivery + budget;
        }
    }
}
