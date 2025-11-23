namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingWithAbBahaResultUpdateDto
    {
        public int Id { get; set; }
        public double SumItems { get; set; }
        public double SumItemsBeforeDiscount { get; set; }
        public double DiscountSum { get; set; }
        public double Consumption { get; set; }
        public double MonthlyConsumption { get; set; }
        public MeterReadingWithAbBahaResultUpdateDto(int id, double sumItems, double sumItemsBeforeDiscount, double discountsum, double consumption, double monthlyConsumption)
        {
            Id=id;
            SumItems=sumItems;
            SumItemsBeforeDiscount=sumItemsBeforeDiscount;
            DiscountSum=discountsum;
            Consumption=consumption;
            MonthlyConsumption=monthlyConsumption;
        }
    }
}
