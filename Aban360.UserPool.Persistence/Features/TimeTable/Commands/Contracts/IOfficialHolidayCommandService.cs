using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IOfficialHolidayCommandService
    {
        Task Add(OfficialHoliday officialHoliday);
        Task Remove(OfficialHoliday officialHoliday);
    }
}
