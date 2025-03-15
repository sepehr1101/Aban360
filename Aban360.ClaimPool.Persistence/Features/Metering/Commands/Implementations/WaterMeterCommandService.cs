using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    internal sealed class WaterMeterCommandService : IWaterMeterCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeter> _wateMere;
        public WaterMeterCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _wateMere = _uow.Set<WaterMeter>();
            _wateMere.NotNull(nameof(_wateMere));
        }

        public async Task Add(WaterMeter waterMeter)
        {
            await _wateMere.AddAsync(waterMeter);
        }

        public async Task Remove(WaterMeter waterMeter)
        {
            _wateMere.Remove(waterMeter);
        }
    }
}