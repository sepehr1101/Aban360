using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class MeterDiameterQueryService : IMeterDiameterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterDiameter> _meterDiameter;
        public MeterDiameterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterDiameter = _uow.Set<MeterDiameter>();
            _meterDiameter.NotNull(nameof(_meterDiameter));
        }

        public async Task<MeterDiameter> Get(short id)
        {
            return await _uow.FindOrThrowAsync<MeterDiameter>(id);
        }

        public async Task<ICollection<MeterDiameter>> Get()
        {
            return await _meterDiameter.ToListAsync();
        }
    }
}
