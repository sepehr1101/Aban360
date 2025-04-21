using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using NetTopologySuite.Index.HPRtree;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface IInvoiceInserterHandler
    {
        Task Handle(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken);
    }
}
