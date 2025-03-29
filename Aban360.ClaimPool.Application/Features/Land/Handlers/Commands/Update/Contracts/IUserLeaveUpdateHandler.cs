using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IUserLeaveUpdateHandler
    {
        Task Handle(UserLeaveUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
