using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class MeterMaterialQueryService : IMeterMaterialQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterMaterial> _meterMaterial;
        public MeterMaterialQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterial = _uow.Set<MeterMaterial>();
            _meterMaterial.NotNull(nameof(_meterMaterial));
        }

        public async Task<MeterMaterial> Get(short id)
        {
            return await _uow.FindOrThrowAsync<MeterMaterial>(id);
        }

        public async Task<ICollection<MeterMaterial>> Get()
        {
            return await _meterMaterial.ToListAsync();
        }
    }
}
