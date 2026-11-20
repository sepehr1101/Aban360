namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    public record invoiceLineItemList
    {
        public ICollection<invoiceLineItemResult> invoiceLineItem { get; set; }
        public string OfferingTitle { get; set; } = default!;

    }
    public record invoiceLineItemResult
    {
        public string Formula { get; set; } = default!;
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public long Amount { get; set; }
        public string LineItemTypeTitle { get; set; } = default!;

    }
}
