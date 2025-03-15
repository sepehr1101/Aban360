using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Implementations
{
    internal sealed class MeterTypeCommandService : IMeterTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterType> _meterType;
        public MeterTypeCommandService(IUnitOfWork uow)
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
    }
}
