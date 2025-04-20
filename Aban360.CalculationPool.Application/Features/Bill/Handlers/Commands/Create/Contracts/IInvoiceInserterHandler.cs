using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using NetTopologySuite.Index.HPRtree;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface IInvoiceInserterHandler
    {
        void Handle(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken);
    }
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
        public double Amount { get; set; }
        public string LineItemTypeTitle { get; set; } = default!;

    }
}
