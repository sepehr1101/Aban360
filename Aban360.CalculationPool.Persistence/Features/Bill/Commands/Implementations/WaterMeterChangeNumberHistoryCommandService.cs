using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    internal sealed class WaterMeterChangeNumberHistoryCommandService : IWaterMeterChangeNumberHistoryCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterChangeNumberHistory> _waterMeterChangeNumberHistory;
        public WaterMeterChangeNumberHistoryCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterChangeNumberHistory = _uow.Set<WaterMeterChangeNumberHistory>();
            _waterMeterChangeNumberHistory.NotNull(nameof(_waterMeterChangeNumberHistory));
        }

        public async Task Add(WaterMeterChangeNumberHistory waterMeterChangeNumberHistory)
        {
            await _waterMeterChangeNumberHistory.AddAsync(waterMeterChangeNumberHistory);
        }

        public async Task Remove(WaterMeterChangeNumberHistory waterMeterChangeNumberHistory)
        {
            _waterMeterChangeNumberHistory.Remove(waterMeterChangeNumberHistory);
        }
    }
}
