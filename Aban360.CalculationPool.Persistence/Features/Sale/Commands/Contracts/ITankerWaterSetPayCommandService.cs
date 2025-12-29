using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts
{
    public interface ITankerWaterSetPayCommandService
    {
        Task Insert(TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto input);   
    }
}
