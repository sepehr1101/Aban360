using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts
{
    public interface ITariffCalculationModeGetSingleHandler
    {
        Task<TariffCalculationModeGetDto> Handle(TariffCalculationModeEnum id, CancellationToken cancellationToken);
    }
}
