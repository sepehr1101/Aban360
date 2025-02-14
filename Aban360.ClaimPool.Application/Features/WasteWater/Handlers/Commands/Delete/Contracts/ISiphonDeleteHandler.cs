using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts
{
    public interface ISiphonDeleteHandler
    {
        Task Handle(SiphonDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
