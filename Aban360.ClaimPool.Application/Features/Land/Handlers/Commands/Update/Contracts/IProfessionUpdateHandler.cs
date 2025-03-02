using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IProfessionUpdateHandler
    {
        Task Handle(ProfessionUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
