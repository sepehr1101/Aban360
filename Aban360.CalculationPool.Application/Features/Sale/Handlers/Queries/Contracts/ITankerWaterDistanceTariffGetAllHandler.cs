using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface ITankerWaterDistanceTariffGetAllHandler
    {
        Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> Handle(CancellationToken cancellationToken);
    }
}
