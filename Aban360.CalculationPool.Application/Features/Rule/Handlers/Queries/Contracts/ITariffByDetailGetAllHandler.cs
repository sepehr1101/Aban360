using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts
{
    public interface ITariffByDetailGetAllHandler
    {
        Task<ICollection<TariffByDetailGetDto>> Handle(CancellationToken cancellationToken);
    }
}
