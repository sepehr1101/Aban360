using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Create.Contracts
{
    public interface IUseStateCreateHandler
    {
        Task Handle(UseStateCreateDto createDto, CancellationToken cancellationToken);
    }
}
