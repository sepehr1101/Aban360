using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class WaterMeterInstallationStructureQueryService : IWaterMeterInstallationMethodQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterInstallationMethod> _waterMeterInstallationStructure;
        public WaterMeterInstallationStructureQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterInstallationStructure = _uow.Set<WaterMeterInstallationMethod>();
            _waterMeterInstallationStructure.NotNull(nameof(_waterMeterInstallationStructure));
        }

        public async Task<WaterMeterInstallationMethod> Get(short id)
        {
            return await _uow.FindOrThrowAsync<WaterMeterInstallationMethod>(id);
        }

        public async Task<ICollection<WaterMeterInstallationMethod>> Get()
        {
            return await _waterMeterInstallationStructure.ToListAsync();
        }
    }
}
