using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface IWaterMeterChangeNumberHistoryCommandService
    {
        Task Add(WaterMeterChangeNumberHistory waterMeterChangeNumberHistory);
        Task Remove(WaterMeterChangeNumberHistory waterMeterChangeNumberHistory);

    }
}
