using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Contracts
{
    public interface IUseStateUpdateHandler
    {
        Task Handle(UseStateUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
