// Test-only contract retained for pre-refactor snapshots.
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts
{
    public interface ISiphonDiameterCreateHandler
    {
        Task Handle(SiphonDiameterCreateDto createDto, CancellationToken cancellationToken);
    }
}
