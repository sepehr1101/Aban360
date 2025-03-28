using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts
{
    public interface IUserWorkdayDeleteHandler
    {
        Task Handle(UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
