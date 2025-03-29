using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IOfficialHolidayCommandService
    {
        Task Add(OfficialHoliday officialHoliday);
        Task Remove(OfficialHoliday officialHoliday);
    }
}
