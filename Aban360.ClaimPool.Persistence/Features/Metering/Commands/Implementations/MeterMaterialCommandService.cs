using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    public class MeterMaterialCommandService : IMeterMaterialCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterMaterial> _meterMaterial;
        public MeterMaterialCommandService(IUnitOfWork uow)
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
    }
}
