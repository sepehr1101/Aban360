using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Implementation
{
    internal sealed class SiphonTypeCommandService : ISiphonTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonType> _siphonType;
        public SiphonTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonType = _uow.Set<SiphonType>();
            _siphonType.NotNull(nameof(_siphonType));
        }

        public async Task Add(SiphonType siphonType)
        {
            await _siphonType.AddAsync(siphonType);
        }

        public async Task Remove(SiphonType siphonType)
        {
            _siphonType.Remove(siphonType);
        }
    }
}
