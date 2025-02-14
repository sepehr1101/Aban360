using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface ISiphonMaterialUpdateHandler
    {
        Task Handle(SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
