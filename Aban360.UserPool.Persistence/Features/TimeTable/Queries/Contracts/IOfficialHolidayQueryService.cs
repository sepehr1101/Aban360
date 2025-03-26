using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts
{
    public interface IOfficialHolidayQueryService
    {
        Task<OfficialHoliday> Get(short id);
        Task<ICollection<OfficialHoliday>> Get();
    }
}
