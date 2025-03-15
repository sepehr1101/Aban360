using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IEstateWaterResourceUpdateHandler
    {
        Task Handle(EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
