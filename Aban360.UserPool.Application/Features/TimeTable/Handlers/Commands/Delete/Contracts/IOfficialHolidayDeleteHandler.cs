using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts
{
    public interface IOfficialHolidayDeleteHandler
    {
        Task Handle(OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
