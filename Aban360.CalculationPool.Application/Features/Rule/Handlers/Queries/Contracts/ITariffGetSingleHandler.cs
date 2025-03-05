using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts
{
    public interface ITariffGetSingleHandler
    {
        Task<TariffGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
