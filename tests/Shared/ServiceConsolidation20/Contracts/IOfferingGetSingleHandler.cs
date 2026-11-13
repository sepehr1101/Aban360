// Test-only contract retained for pre-refactor snapshots.
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IOfferingGetSingleHandler
    {
        Task<OfferingGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
