using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IOfficialHolidayGetAllHandler
    {
        Task<ICollection<OfficialHolidayGetDto>> Handle(CancellationToken cancellationToken);
    }
}
