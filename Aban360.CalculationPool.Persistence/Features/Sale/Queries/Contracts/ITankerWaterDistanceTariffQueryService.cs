using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface ITankerWaterDistanceTariffQueryService
    {
        Task<TankerWaterDistanceTariffOutputDto> Get(short id);
        Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> Get();
    }
}
