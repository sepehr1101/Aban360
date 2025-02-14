using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface ISiphonTypeUpdateHandler
    {
        Task Handle(SiphonTypeUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
