using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface ISiphonUpdateHandler
    {
        Task Handle(SiphonUpdateDto updateDto, CancellationToken cancellationToken);
    }


}
