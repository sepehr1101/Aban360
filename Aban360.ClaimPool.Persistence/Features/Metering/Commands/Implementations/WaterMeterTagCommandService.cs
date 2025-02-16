using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    public class WaterMeterTagCommandService : IWaterMeterTagCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterTag> _waterMeterTag;
        public WaterMeterTagCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterTag = _uow.Set<WaterMeterTag>();
            _waterMeterTag.NotNull(nameof(_waterMeterTag));
        }

        public async Task Add(WaterMeterTag waterMeterTag)
        {
            await _waterMeterTag.AddAsync(waterMeterTag);
        }

        public async Task Remove(WaterMeterTag waterMeterTag)
        {
            _waterMeterTag.Remove(waterMeterTag);
        }
    }
}
