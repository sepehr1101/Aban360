using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Implementation
{
    internal sealed class WaterMeterSiphonCommandService : IWaterMeterSiphonCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterSiphon> _waterMeterSiphon;
        public WaterMeterSiphonCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterSiphon = _uow.Set<WaterMeterSiphon>();
            _waterMeterSiphon.NotNull(nameof(_waterMeterSiphon));
        }

        public async Task Add(WaterMeterSiphon waterMeterSiphon)
        {
            await _waterMeterSiphon.AddAsync(waterMeterSiphon);
        }

        public async Task Remove(WaterMeterSiphon waterMeterSiphon)
        {
            _waterMeterSiphon.Remove(waterMeterSiphon);
        }
    }
}
