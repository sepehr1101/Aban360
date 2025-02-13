using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Implementations
{
    public class ZoneCommandService : IZoneCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Zone> _zone;
        public ZoneCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _zone = _uow.Set<Zone>();
            _zone.NotNull(nameof(_zone));
        }

        public async Task Add(Zone zone)
        {
            await _zone.AddAsync(zone);
        }

        public async Task Remove(Zone zone)
        {
            _zone.Remove(zone);
        }
    }

}
