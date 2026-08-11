using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    public interface ITankerTariffGetAllHandler
    {
        Task<IEnumerable<TankerTariffGetDto>> Handle(CancellationToken cancellationToken);
    }
}
