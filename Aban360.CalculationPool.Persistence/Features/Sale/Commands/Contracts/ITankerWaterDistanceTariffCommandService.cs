using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts
{
    public interface ITankerWaterDistanceTariffCommandService
    {
        Task Create(TankerWaterDistanceTariffCreateDto input);
        Task Delete(DeleteDto input);
        Task Update(TankerWaterDistanceTariffInputDto input);
    }
}
