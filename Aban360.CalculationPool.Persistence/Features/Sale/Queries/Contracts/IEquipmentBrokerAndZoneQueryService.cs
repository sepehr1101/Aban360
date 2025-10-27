using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IEquipmentBrokerAndZoneQueryService
    {
        Task<EquipmentBrokerOutputDto> Get(int zoneId);
    }
}
