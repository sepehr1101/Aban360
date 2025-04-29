using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    internal sealed class WaterMeterInstallationStructureCommandService : IWaterMeterInstallationStructureCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterInstallationStructure> _waterMeterInstallationStructure;
        public WaterMeterInstallationStructureCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterInstallationStructure = _uow.Set<WaterMeterInstallationStructure>();
            _waterMeterInstallationStructure.NotNull(nameof(_waterMeterInstallationStructure));
        }

        public async Task Add(WaterMeterInstallationStructure waterMeterInstallationStructure)
        {
            await _waterMeterInstallationStructure.AddAsync(waterMeterInstallationStructure);
        }

        public async Task Remove(WaterMeterInstallationStructure waterMeterInstallationStructure)
        {
            _waterMeterInstallationStructure.Remove(waterMeterInstallationStructure);
        }
    }
}
