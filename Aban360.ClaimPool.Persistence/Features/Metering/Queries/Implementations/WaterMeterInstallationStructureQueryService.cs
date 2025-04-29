using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class WaterMeterInstallationStructureQueryService : IWaterMeterInstallationStructureQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterInstallationStructure> _waterMeterInstallationStructure;
        public WaterMeterInstallationStructureQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterInstallationStructure = _uow.Set<WaterMeterInstallationStructure>();
            _waterMeterInstallationStructure.NotNull(nameof(_waterMeterInstallationStructure));
        }

        public async Task<WaterMeterInstallationStructure> Get(WaterMeterInstallationStructureEnum id)
        {
            return await _uow.FindOrThrowAsync<WaterMeterInstallationStructure>(id);
        }

        public async Task<ICollection<WaterMeterInstallationStructure>> Get()
        {
            return await _waterMeterInstallationStructure.ToListAsync();
        }
    }
}
