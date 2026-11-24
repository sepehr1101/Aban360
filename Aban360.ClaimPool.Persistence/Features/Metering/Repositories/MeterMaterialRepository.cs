using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Repositories
{
    internal sealed class MeterMaterialRepository : IMeterMaterialCommandService, IMeterMaterialQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterMaterial> _meterMaterial;
        public MeterMaterialRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterMaterial = _uow.Set<MeterMaterial>();
            _meterMaterial.NotNull(nameof(_meterMaterial));
        }

        public async Task Add(MeterMaterial meterMaterial)
        {
            await _meterMaterial.AddAsync(meterMaterial);
        }

        public async Task Remove(MeterMaterial meterMaterial)
        {
            _meterMaterial.Remove(meterMaterial);
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
