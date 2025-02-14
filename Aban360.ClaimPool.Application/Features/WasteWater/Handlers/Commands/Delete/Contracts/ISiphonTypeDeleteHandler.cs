using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts
{
    public interface ISiphonTypeDeleteHandler
    {
        Task Handle(SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
