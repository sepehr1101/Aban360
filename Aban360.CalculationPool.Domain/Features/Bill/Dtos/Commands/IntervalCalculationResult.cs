using Aban360.CalculationPool.Domain.Features.Rule.Entties;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record IntervalCalculationResult()
    {
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public string Formula { get; set; } = default!;
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public double Amount { get; set; }
        public string OfferingTitle { get; set; } = default!;
        public string LineItemTypeTitle { get; set; } = default!;
        public IntervalCalculationResult(Tariff tariff, double amount):this()
        {
            Amount = amount;
            Formula = tariff.Formula;
            Consumption = tariff.Consumption; 
            Duration = tariff.Duration;
            FromDate = tariff.FromDateJalali;
            ToDate = tariff.ToDateJalali;
            LineItemTypeTitle = tariff.LineItemType.Title;
            OfferingTitle = tariff.Offering.Title;
        }
    }
}