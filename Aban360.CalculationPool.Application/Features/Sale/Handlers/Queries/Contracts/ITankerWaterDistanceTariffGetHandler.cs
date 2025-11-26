using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface ITankerWaterDistanceTariffGetHandler
    {
        Task<TankerWaterDistanceTariffOutputDto> Handle(SearchById input, CancellationToken cancellationToken);
    }
}
