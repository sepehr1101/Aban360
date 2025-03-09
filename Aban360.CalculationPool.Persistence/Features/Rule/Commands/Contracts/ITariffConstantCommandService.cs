using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts
{
    public interface ITariffConstantCommandService
    {
        Task Add(TariffConstant tariffConstant);
        Task Remove(TariffConstant tariffConstant);
    }
}
