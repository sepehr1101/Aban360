using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts
{
    public interface ISiphonTypeCreateHandler
    {
        Task Handle(SiphonTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
