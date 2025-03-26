using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUserLeaveGetAllHandler
    {
        Task<ICollection<UserLeaveGetDto>> Handle(CancellationToken cancellationToken);
    }
}
