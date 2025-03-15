using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class MeterUseTypeQueryService : IMeterUseTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterUseType> _meterUserType;
        public MeterUseTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterUserType = _uow.Set<MeterUseType>();
            _meterUserType.NotNull(nameof(_meterUserType));
        }

        public async Task<MeterUseType> Get(MeterUseTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<MeterUseType>(id);
        }

        public async Task<ICollection<MeterUseType>> Get()
        {
            return await _meterUserType.ToListAsync();
        }
    }
}
