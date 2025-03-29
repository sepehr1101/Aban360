using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IOfficialHolidayUpdateHandler
    {
        Task Handle(OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
