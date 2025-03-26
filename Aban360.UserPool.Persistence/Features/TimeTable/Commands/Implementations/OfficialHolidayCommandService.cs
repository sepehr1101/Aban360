using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Implementations
{
    internal sealed class OfficialHolidayCommandService : IOfficialHolidayCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfficialHoliday> _officialHoliday;
        public OfficialHolidayCommandService(IUnitOfWork uow)
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
    }
}
