using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ITankerTariffQueryService
    {
        Task<TankerTariffGetDto> Get(int zoneId);
        Task<IEnumerable<TankerTariffGetDto>> Get();
    }
}
