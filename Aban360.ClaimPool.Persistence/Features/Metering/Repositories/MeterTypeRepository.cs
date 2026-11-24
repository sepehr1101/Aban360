using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Repositories
{
    internal sealed class MeterTypeRepository : IMeterTypeCommandService, IMeterTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterType> _meterType;
        public MeterTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterType = _uow.Set<MeterType>();
            _meterType.NotNull(nameof(_meterType));
        }

        public async Task Add(MeterType meterType)
        {
            await _meterType.AddAsync(meterType);
        }

        public async Task Remove(MeterType meterType)
        {
            _meterType.Remove(meterType);
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
