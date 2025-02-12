using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    public class MeterUseTypeCommandService : IMeterUseTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterUseType> _meterUserType;
        public MeterUseTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterUserType = _uow.Set<MeterUseType>();
            _meterUserType.NotNull(nameof(_meterUserType));
        }

        public async Task Add(MeterUseType meterUserType)
        {
            await _meterUserType.AddAsync(meterUserType);
        }

        public async Task Remove(MeterUseType meterUserType)
        {
            _meterUserType.Remove(meterUserType);
        }
    }
}
