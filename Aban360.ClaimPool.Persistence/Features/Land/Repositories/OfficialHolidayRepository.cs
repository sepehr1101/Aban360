using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class OfficialHolidayRepository : IOfficialHolidayCommandService, IOfficialHolidayQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfficialHoliday> _officialHoliday;
        public OfficialHolidayRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _officialHoliday = _uow.Set<OfficialHoliday>();
            _officialHoliday.NotNull(nameof(_officialHoliday));
        }

        public async Task Add(OfficialHoliday officialHoliday)
        {
            await _officialHoliday.AddAsync(officialHoliday);
        }

        public async Task Remove(OfficialHoliday officialHoliday)
        {
            _officialHoliday.Remove(officialHoliday);
        }

        public async Task<OfficialHoliday> Get(short id)
        {
            return await _uow.FindOrThrowAsync<OfficialHoliday>(id);
        }

        public async Task<ICollection<OfficialHoliday>> Get()
        {
            return await _officialHoliday.ToListAsync();
        }

    }
}
