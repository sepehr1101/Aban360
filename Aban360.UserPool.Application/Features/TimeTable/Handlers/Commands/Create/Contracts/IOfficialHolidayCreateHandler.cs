using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts
{
    public interface IOfficialHolidayCreateHandler
    {
        Task Handle(OfficialHolidayCreateDto createDto, CancellationToken cancellationToken);
    }
}
