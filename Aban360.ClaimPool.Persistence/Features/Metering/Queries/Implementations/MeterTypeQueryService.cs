using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class MeterTypeQueryService : IMeterTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterType> _meterType;
        public MeterTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterType = _uow.Set<MeterType>();
            _meterType.NotNull(nameof(_meterType));
        }

        public async Task<MeterType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<MeterType>(id);
        }

        public async Task<ICollection<MeterType>> Get()
        {
            return await _meterType.ToListAsync();
        }
    }
}
