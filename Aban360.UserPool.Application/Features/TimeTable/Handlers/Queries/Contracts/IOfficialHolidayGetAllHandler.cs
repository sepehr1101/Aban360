using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IOfficialHolidayGetAllHandler
    {
        Task<ICollection<OfficialHolidayGetDto>> Handle(CancellationToken cancellationToken);
    }
}
