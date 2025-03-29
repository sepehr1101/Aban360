using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Microsoft.EntityFrameworkCore;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Implementations
{
    internal sealed class OfficialHolidayQueryService : IOfficialHolidayQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfficialHoliday> _officialHoliday;
        public OfficialHolidayQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _officialHoliday = _uow.Set<OfficialHoliday>();
            _officialHoliday.NotNull(nameof(_officialHoliday));
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
