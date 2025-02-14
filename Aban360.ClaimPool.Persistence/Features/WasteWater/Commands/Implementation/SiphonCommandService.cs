using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Implementation
{
    public class SiphonCommandService : ISiphonCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Siphon> _siphon;
        public SiphonCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphon = _uow.Set<Siphon>();
            _siphon.NotNull(nameof(_siphon));
        }

        public async Task Add(Siphon siphon)
        {
            await _siphon.AddAsync(siphon);
        }

        public async Task Remove(Siphon siphon)
        {
            _siphon.Remove(siphon);
        }
    }
}
