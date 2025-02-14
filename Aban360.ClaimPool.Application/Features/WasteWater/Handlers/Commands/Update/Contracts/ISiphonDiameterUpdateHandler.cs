using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts
{
    public interface ISiphonDiameterUpdateHandler
    {
        Task Handle(SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
