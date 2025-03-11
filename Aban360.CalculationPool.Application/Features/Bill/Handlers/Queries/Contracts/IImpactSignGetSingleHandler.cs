using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IImpactSignGetSingleHandler
    {
        Task<ImpactSignGetDto> Handle(ImpactSignEnum id, CancellationToken cancellationToken);
    }
}
